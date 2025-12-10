using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaEmpresas.Data;
using SistemaEmpresas.Middleware;
using SistemaEmpresas.Services;
using Microsoft.Extensions.Hosting.WindowsServices;

// Desabilitar avisos de TLS 1.0 e suportar múltiplas versões
AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.UseLegacyEncryptionMode", true);
AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.DisableTLSWarning", true);
System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;

// Configuração para Windows Service
var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() 
        ? AppContext.BaseDirectory 
        : Directory.GetCurrentDirectory()
};

var builder = WebApplication.CreateBuilder(options);

// Adicionar suporte a Windows Service
builder.Host.UseWindowsService();

// Add services to the container.

// Configuração de Controllers com filtro Anti-XSS
builder.Services.AddControllers(options =>
{
    // Filtro global para validação de XSS em todos os inputs
    options.Filters.Add<SistemaEmpresas.Middleware.AntiXssValidationFilter>();
});

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sistema Empresas API - Multi-Tenant",
        Version = "v1",
        Description = "API com suporte a Multi-Tenant por banco de dados e autenticação JWT"
    });

    // Configuração para autenticação JWT no Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuração do CORS para o frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", 
                "http://localhost:5174",
                "https://192.168.167.125:5001",
                "http://192.168.167.125:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configuração do HttpContextAccessor (necessário para o AppDbContext)
builder.Services.AddHttpContextAccessor();

// Configuração do Memory Cache para Tenants
builder.Services.AddMemoryCache();

// Configuração do TenantDbContext (banco que armazena os tenants)
builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao"), sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
    }));

// Configuração do AppDbContext (banco dos dados da aplicação - troca dinamicamente)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao"), sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
    }));

// Registro do TenantService
builder.Services.AddScoped<ITenantService, TenantService>();

// Registro do AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Registro do CacheService (cache centralizado com suporte a multi-tenant)
builder.Services.AddSingleton<ICacheService, CacheService>();

// Configuração de Cache Distribuído (para ClassTrib Sync)
builder.Services.AddDistributedMemoryCache(); // Em produção, usar Redis

// Carregar certificado digital para API SVRS
// Por padrão usa o certificado da Irrigação, mas pode ser alterado por tenant
var certPath = Path.Combine(builder.Environment.ContentRootPath, 
    builder.Configuration["CertificadosDigitais:Irrigacao:CaminhoArquivo"] ?? "certificado\\Irrigacao.pfx");
var certSenha = builder.Configuration["CertificadosDigitais:Irrigacao:Senha"] ?? "";

X509Certificate2? certificadoDigital = null;
if (File.Exists(certPath))
{
    try
    {
        certificadoDigital = new X509Certificate2(certPath, certSenha, X509KeyStorageFlags.MachineKeySet);
        Console.WriteLine($"✅ Certificado digital carregado: {certificadoDigital.Subject}");
        Console.WriteLine($"   Válido de {certificadoDigital.NotBefore:dd/MM/yyyy} até {certificadoDigital.NotAfter:dd/MM/yyyy}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Erro ao carregar certificado digital: {ex.Message}");
    }
}
else
{
    Console.WriteLine($"⚠️ Certificado digital não encontrado em: {certPath}");
}

// Registro do HttpClient para ClassTribApiClient
builder.Services.AddHttpClient<ClassTribApiClient>(client =>
{
    // IMPORTANTE: BaseAddress DEVE terminar com "/" para concatenação correta de endpoints
    client.BaseAddress = new Uri("https://cff.svrs.rs.gov.br/api/v1/");
    client.Timeout = TimeSpan.FromSeconds(60);
    
    // Headers necessários para evitar 403 Forbidden da API SVRS
    // A API verifica headers típicos de navegador para prevenir bots
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
    client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
    client.DefaultRequestHeaders.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
    client.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
    client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
    client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
    client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "cross-site");
    client.DefaultRequestHeaders.Add("Origin", "https://cff.svrs.rs.gov.br");
    client.DefaultRequestHeaders.Add("Referer", "https://cff.svrs.rs.gov.br/");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    // Handler configurado com certificado digital para autenticação na API SVRS
    var handler = new HttpClientHandler
    {
        AutomaticDecompression = System.Net.DecompressionMethods.All,
        AllowAutoRedirect = true,
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer()
    };
    
    // Adicionar certificado digital se disponível
    if (certificadoDigital != null)
    {
        handler.ClientCertificates.Add(certificadoDigital);
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        Console.WriteLine("✅ Certificado digital anexado ao HttpClient para API SVRS");
    }
    
    return handler;
});

// Registro do HttpClient genérico para uso em controllers (ex: consulta CNPJ)
builder.Services.AddHttpClient();

// Registro do Repository de Classificação Fiscal
builder.Services.AddScoped<SistemaEmpresas.Repositories.IClassificacaoFiscalRepository, 
    SistemaEmpresas.Repositories.ClassificacaoFiscalRepository>();

// Registro do Repository de ClassTrib (Classificações Tributárias SVRS)
builder.Services.AddScoped<SistemaEmpresas.Repositories.IClassTribRepository, 
    SistemaEmpresas.Repositories.ClassTribRepository>();

// Registro do Repository de Produtos
builder.Services.AddScoped<SistemaEmpresas.Repositories.IProdutoRepository,
    SistemaEmpresas.Repositories.ProdutoRepository>();

// Registro do Repository de Nota Fiscal
builder.Services.AddScoped<SistemaEmpresas.Repositories.INotaFiscalRepository,
    SistemaEmpresas.Repositories.NotaFiscalRepository>();

// Registro do Repository e Service de Gerenciamento de Usuários
builder.Services.AddScoped<SistemaEmpresas.Repositories.IUsuarioManagementRepository,
    SistemaEmpresas.Repositories.UsuarioManagementRepository>();
builder.Services.AddScoped<IUsuarioManagementService, UsuarioManagementService>();

// Registro do Repository e Service de Permissões por Tela
builder.Services.AddScoped<SistemaEmpresas.Repositories.IPermissoesTelaRepository,
    SistemaEmpresas.Repositories.PermissoesTelaRepository>();
builder.Services.AddScoped<IPermissoesTelaService, PermissoesTelaService>();

// Registro do Repository e Service de Grupos/Usuários do Sistema Web (Nova estrutura independente do VB6)
builder.Services.AddScoped<SistemaEmpresas.Repositories.IGrupoUsuarioRepository,
    SistemaEmpresas.Repositories.GrupoUsuarioRepository>();
builder.Services.AddScoped<IGrupoUsuarioService, GrupoUsuarioService>();

// Registro do ClassTrib Sync Service
builder.Services.AddScoped<ClassTribSyncService>();

// Registro do Repository e Service de Auditoria (Logs)
builder.Services.AddScoped<SistemaEmpresas.Repositories.ILogAuditoriaRepository,
    SistemaEmpresas.Repositories.LogAuditoriaRepository>();
builder.Services.AddScoped<ILogAuditoriaService, LogAuditoriaService>();

// Registro do Serviço de Logs de Segurança
builder.Services.AddScoped<ILogSegurancaService, LogSegurancaService>();

// Registro do Serviço de Cálculo de Impostos
builder.Services.AddScoped<SistemaEmpresas.Services.Fiscal.ICalculoImpostoService,
    SistemaEmpresas.Services.Fiscal.CalculoImpostoService>();

// ===========================================
// Módulo de Transporte - Services
// ===========================================
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IVeiculoService,
    SistemaEmpresas.Services.Transporte.VeiculoService>();
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IReboqueService,
    SistemaEmpresas.Services.Transporte.ReboqueService>();
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IViagemService,
    SistemaEmpresas.Services.Transporte.ViagemService>();
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IDespesaViagemService,
    SistemaEmpresas.Services.Transporte.DespesaViagemService>();
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IReceitaViagemService,
    SistemaEmpresas.Services.Transporte.ReceitaViagemService>();
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IManutencaoVeiculoService,
    SistemaEmpresas.Services.Transporte.ManutencaoVeiculoService>();
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IManutencaoPecaService,
    SistemaEmpresas.Services.Transporte.ManutencaoPecaService>();

// Motorista
builder.Services.AddScoped<SistemaEmpresas.Services.Transporte.IMotoristaService,
    SistemaEmpresas.Services.Transporte.MotoristaService>();

// Configuração da Autenticação JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey não configurado");
var key = Encoding.UTF8.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Em produção, mude para true
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Remove tolerância de tempo
    };
});

builder.Services.AddAuthorization();

// ===========================================
// Rate Limiting - Proteção contra abuso de API
// ===========================================
builder.Services.AddRateLimiter(options =>
{
    // Política Global - Limita requisições por IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        // Usa IP do cliente ou "unknown" se não disponível
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,           // Máximo 100 requisições
                Window = TimeSpan.FromMinutes(1), // Por minuto
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5               // Fila de até 5 requisições
            });
    });

    // Política específica para Login - Mais restritiva (proteção contra brute force)
    options.AddFixedWindowLimiter("login", limiterOptions =>
    {
        limiterOptions.AutoReplenishment = true;
        limiterOptions.PermitLimit = 5;           // Máximo 5 tentativas
        limiterOptions.Window = TimeSpan.FromMinutes(5); // A cada 5 minutos
        limiterOptions.QueueLimit = 0;            // Sem fila para login
    });

    // Política para endpoints de consulta intensiva
    options.AddSlidingWindowLimiter("consulta", limiterOptions =>
    {
        limiterOptions.AutoReplenishment = true;
        limiterOptions.PermitLimit = 30;          // Máximo 30 requisições
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.SegmentsPerWindow = 6;     // 6 segmentos de 10 segundos
        limiterOptions.QueueLimit = 2;
    });

    // Política para escrita de dados (Create/Update/Delete) - Moderada
    options.AddTokenBucketLimiter("escrita", limiterOptions =>
    {
        limiterOptions.AutoReplenishment = true;
        limiterOptions.TokenLimit = 20;           // Bucket com 20 tokens
        limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
        limiterOptions.TokensPerPeriod = 5;       // Repõe 5 tokens a cada 30 segundos
        limiterOptions.QueueLimit = 2;
    });

    // Callback quando requisição é rejeitada pelo rate limiter
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        
        var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue)
            ? retryAfterValue.TotalSeconds
            : 60;
        
        context.HttpContext.Response.Headers.RetryAfter = retryAfter.ToString("F0");
        
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            mensagem = "Muitas requisições. Tente novamente em breve.",
            retryAfterSeconds = retryAfter
        }, cancellationToken);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Empresas API v1");
        options.RoutePrefix = "swagger"; // Swagger em /swagger
    });
}

// Em produção, servir arquivos estáticos do frontend (pasta wwwroot)
app.UseDefaultFiles(); // Serve index.html por padrão
app.UseStaticFiles();  // Serve arquivos estáticos (CSS, JS, etc)

app.UseHttpsRedirection();

// ⚠️ Headers de Segurança - DEVE ser um dos primeiros middlewares
app.UseSecurityHeaders();

// ⚠️ IMPORTANTE: Middleware de exceções global DEVE ser o primeiro
app.UseGlobalExceptionHandler();

// Habilita CORS
app.UseCors("AllowFrontend");

// Rate Limiting - ANTES do middleware de Tenant e Autenticação
app.UseRateLimiter();

// ⚠️ IMPORTANTE: Middleware do Tenant ANTES da autorização e dos controllers
app.UseTenantMiddleware();

// Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

// ⚠️ Middleware de Auditoria - Registra todas as ações dos usuários
app.UseAuditMiddleware();

app.MapControllers();

// Fallback para SPA - rotas que não são API retornam o index.html
app.MapFallbackToFile("index.html");

// Seed Database
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.InitializeAsync(scope.ServiceProvider);
}

app.Run();
