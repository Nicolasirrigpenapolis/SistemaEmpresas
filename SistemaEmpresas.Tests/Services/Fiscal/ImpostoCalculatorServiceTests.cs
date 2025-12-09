using Xunit;
using SistemaEmpresas.Services.Fiscal;
using SistemaEmpresas.Models.Fiscal;

namespace SistemaEmpresas.Tests.Services.Fiscal
{
    public class ImpostoCalculatorServiceTests
    {
        private readonly ImpostoCalculatorService _service;

        public ImpostoCalculatorServiceTests()
        {
            _service = new ImpostoCalculatorService();
        }

        [Fact]
        public void Deve_Calcular_ICMS_Simples_Dentro_Estado()
        {
            var ctx = new ContextoCalculo
            {
                Oq = 7, // Valor ICMS
                VrTotal = 1000m,
                AliqICMS = 18m,
                ForaDoEstado = false,
                Reducao = false
            };

            var resultado = (decimal)_service.CalculaImposto(ctx);
            Assert.Equal(180m, resultado);
        }

        [Fact]
        public void Deve_Calcular_IBS_Reforma_Tributaria()
        {
            var ctx = new ContextoCalculo
            {
                Oq = 16, // Valor IBS
                VrTotal = 1000m
            };

            var resultado = (decimal)_service.CalculaImposto(ctx);
            // 1000 * 0.001 = 1.00
            Assert.Equal(1.00m, resultado);
        }

        [Fact]
        public void Deve_Calcular_CBS_Reforma_Tributaria()
        {
            var ctx = new ContextoCalculo
            {
                Oq = 17, // Valor CBS
                VrTotal = 1000m
            };

            var resultado = (decimal)_service.CalculaImposto(ctx);
            // 1000 * 0.9% = 9.00
            Assert.Equal(9.00m, resultado);
        }

        [Fact]
        public void Deve_Calcular_ST_Com_MVA_Ajustado()
        {
            var ctx = new ContextoCalculo
            {
                Oq = 14, // Valor ICMS ST
                VrTotal = 1000m,
                AliqICMS = 18m, // Interna Destino
                AliqInterestadual = 12m, // Interestadual
                IVAOriginal = 50m,
                Substituicao = true,
                ForaDoEstado = true
            };

            // IVA Ajustado:
            // Fator1 = 1.5
            // Fator2 = 0.88
            // Fator3 = 0.82
            // (1.5 * 0.88 / 0.82) - 1 = 0.6097... * 100 = 60.98%
            
            // BC ST = 1000 * 1.6098 = 1609.80
            // ICMS Proprio = 1000 * 12% = 120
            // ST = (1609.80 * 18%) - 120 = 289.76 - 120 = 169.76
            
            var resultado = (decimal)_service.CalculaImposto(ctx);
            Assert.True(resultado > 160m && resultado < 180m); // Validação aproximada
        }
    }
}
