import React, { useState, useEffect, useRef, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { 
  User, Lock, AlertCircle, Eye, EyeOff, LogIn, CheckCircle2, 
  KeyRound, ChevronLeft, Shield, Building2, Activity
} from 'lucide-react';
import { APP_VERSION, APP_NAME, COPYRIGHT_YEAR } from '../../config/version';

const Login: React.FC = () => {
  const [empresa, setEmpresa] = useState('');
  const [usuario, setUsuario] = useState(''); // valor real para login
  const [usuarioDisplay, setUsuarioDisplay] = useState(''); // valor formatado para exibição
  const [senha, setSenha] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [mounted, setMounted] = useState(false);
  const [capsLockOn, setCapsLockOn] = useState(false);
  const [loginSuccess, setLoginSuccess] = useState(false);
  const [step, setStep] = useState<'empresa' | 'credenciais'>('empresa');
  const [hoveredCard, setHoveredCard] = useState<string | null>(null);
  
  const usuarioInputRef = useRef<HTMLInputElement>(null);
  const senhaInputRef = useRef<HTMLInputElement>(null);
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  // Redireciona para dashboard se já estiver autenticado
  useEffect(() => {
    if (isAuthenticated) {
      navigate('/dashboard', { replace: true });
    }
  }, [isAuthenticated, navigate]);

  // Memoizar os valores aleatórios dos dots para não recalcular a cada re-render
  const floatingDots = useMemo(() => 
    [...Array(25)].map((_, i) => ({
      id: i,
      left: `${5 + Math.random() * 90}%`,
      width: `${6 + Math.random() * 8}px`,
      height: `${6 + Math.random() * 8}px`,
      opacity: 0.3 + Math.random() * 0.4,
      animationDelay: `${Math.random() * 8}s`,
      animationDuration: `${12 + Math.random() * 10}s`,
    }))
  , []);

  // Função para capitalizar: primeira letra e após espaços
  const capitalizeWords = (text: string): string => {
    return text
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  };

  // Handler do input de usuário
  const handleUsuarioChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const rawValue = e.target.value;
    setUsuario(rawValue); // valor original para login
    setUsuarioDisplay(capitalizeWords(rawValue)); // valor formatado para exibição
  };

  useEffect(() => {
    setMounted(true);
    // Sempre inicia na seleção de empresa - sem auto-login
  }, []);


  const empresas = [
    { 
      value: 'irrigacao', 
      label: 'Irrigação Penápolis',
      subtitle: 'Sistema de Gestão Agrícola',
      logo: '/logos/logo_IP.png',
    },
    { 
      value: 'chinellato', 
      label: 'Chinellato Transportes',
      subtitle: 'Logística e Frotas',
      logo: '/logos/logo_chinellato.png',
    },
  ];

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    setIsLoading(true);
    try {
      // Backend valida campos obrigatórios
      await login(empresa, usuario, senha);
      
      setLoginSuccess(true);
      setTimeout(() => navigate('/dashboard'), 600);
    } catch (err: any) {
      setError(err.message || 'Credenciais inválidas');
      // Limpa apenas a senha e mantém foco para tentar novamente
      setSenha('');
      setTimeout(() => senhaInputRef.current?.focus(), 100);
    } finally {
      setIsLoading(false);
    }
  };

  const handleEmpresaClick = (value: string) => {
    setEmpresa(value);
    setError('');
    setTimeout(() => {
      setStep('credenciais');
      setTimeout(() => usuarioInputRef.current?.focus(), 200);
    }, 150);
  };

  const handleVoltarEmpresa = () => {
    setStep('empresa');
    setUsuario('');
    setUsuarioDisplay('');
    setSenha('');
    setError('');
  };

  const getEmpresaSelecionada = () => empresas.find(emp => emp.value === empresa);

  const handleCapsLock = (e: React.KeyboardEvent) => setCapsLockOn(e.getModifierState('CapsLock'));

  return (
    <div 
      className="min-h-screen relative overflow-hidden" 
      data-theme="light"
      style={{ background: 'linear-gradient(to bottom right, #f8fafc, #eff6ff, #eef2ff)' }}
    >
      {/* Animated Background Effects */}
      <div className="absolute inset-0">
        {/* Moving gradient orbs - AZUL mais visível */}
        <div className="absolute top-0 left-0 w-full h-full pointer-events-none">
          <div className="absolute top-[10%] left-[5%] w-[400px] h-[400px] bg-blue-400/40 rounded-full blur-[80px] animate-blob" />
          <div className="absolute top-[60%] right-[5%] w-[350px] h-[350px] bg-blue-500/35 rounded-full blur-[80px] animate-blob animation-delay-2000" />
          <div className="absolute bottom-[5%] left-[40%] w-[300px] h-[300px] bg-indigo-400/30 rounded-full blur-[70px] animate-blob animation-delay-4000" />
          <div className="absolute top-[30%] right-[30%] w-[250px] h-[250px] bg-cyan-400/25 rounded-full blur-[60px] animate-blob animation-delay-3000" />
        </div>
        
        {/* Floating circles - bordas azuis */}
        <div className="absolute inset-0 overflow-hidden pointer-events-none">
          <div className="absolute top-[15%] left-[15%] w-64 h-64 border-2 border-blue-300/40 rounded-full animate-spin-slow" />
          <div className="absolute bottom-[25%] right-[10%] w-48 h-48 border-2 border-blue-400/30 rounded-full animate-spin-slow animation-delay-2000" style={{ animationDirection: 'reverse' }} />
          <div className="absolute top-[50%] left-[60%] w-32 h-32 border border-indigo-300/40 rounded-full animate-spin-slow animation-delay-4000" />
        </div>

        {/* Floating blue dots going up */}
        <div className="absolute inset-0 overflow-hidden pointer-events-none">
          {floatingDots.map((dot) => (
            <div
              key={dot.id}
              className="absolute rounded-full animate-float-up"
              style={{
                left: dot.left,
                bottom: `-20px`,
                width: dot.width,
                height: dot.height,
                background: `rgba(59, 130, 246, ${dot.opacity})`,
                animationDelay: dot.animationDelay,
                animationDuration: dot.animationDuration,
              }}
            />
          ))}
        </div>

        {/* Grid pattern */}
        <div 
          className="absolute inset-0 opacity-[0.04] pointer-events-none"
          style={{
            backgroundImage: `
              linear-gradient(rgba(59, 130, 246, 0.3) 1px, transparent 1px),
              linear-gradient(90deg, rgba(59, 130, 246, 0.3) 1px, transparent 1px)
            `,
            backgroundSize: '40px 40px',
          }}
        />
      </div>

      {/* Main Content */}
      <div className="relative min-h-screen flex">
        {/* Left Side - Welcome Panel (Hidden on mobile) */}
        <div className="hidden lg:flex lg:w-1/2 xl:w-[55%] relative flex-col items-center justify-center p-12">
          <div className={`max-w-xl transition-all duration-1000 ${mounted ? 'opacity-100 translate-x-0' : 'opacity-0 -translate-x-10'}`}>
            
            {/* Welcome Content */}
            <div>
              <p className="text-sm font-medium text-indigo-600 mb-3 tracking-wide">
                PORTAL CORPORATIVO
              </p>
              
              <h1 className="text-4xl xl:text-5xl font-bold text-slate-900 leading-tight mb-6">
                Bem-vindo,
                <br />
                <span className="text-slate-900">colaborador.</span>
              </h1>
              
              <p className="text-lg text-slate-500 leading-relaxed mb-10 max-w-md">
                Acesse sua conta para continuar trabalhando. Selecione a empresa e insira suas credenciais.
              </p>

              {/* Decorative element */}
              <div className="flex items-center gap-4 mb-10">
                <div className="h-px flex-1 bg-gradient-to-r from-gray-200 to-transparent"></div>
              </div>

              {/* Info cards */}
              <div className="flex gap-6">
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-xl bg-indigo-50 flex items-center justify-center">
                    <Shield className="w-5 h-5 text-indigo-600" />
                  </div>
                  <div>
                    <p className="text-sm font-medium text-gray-700">Acesso Seguro</p>
                    <p className="text-xs text-gray-400">Dados criptografados</p>
                  </div>
                </div>
                
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-xl bg-emerald-50 flex items-center justify-center">
                    <Activity className="w-5 h-5 text-emerald-600" />
                  </div>
                  <div>
                    <p className="text-sm font-medium text-gray-700">Sistema Online</p>
                    <p className="text-xs text-gray-400">Operando normalmente</p>
                  </div>
                </div>
              </div>
            </div>
            
          </div>

          {/* Footer - Copyright (posicionado no fundo do painel esquerdo) */}
          <div className="absolute bottom-8 left-12 right-12">
            <div className="flex items-center gap-3 mb-3">
              <div className="h-px flex-1 bg-gradient-to-r from-gray-300/40 to-transparent" />
            </div>
            <p className="text-sm font-medium text-slate-500">
              © {COPYRIGHT_YEAR} {APP_NAME}
            </p>
            <p className="text-xs text-gray-400 mt-0.5">
              Versão {APP_VERSION}
            </p>
          </div>
        </div>

        {/* Right Side - Login Form */}
        <div className="w-full lg:w-1/2 xl:w-[45%] flex items-center justify-center p-4 sm:p-6 lg:p-12">
          <div className={`w-full max-w-md transition-all duration-700 ${mounted ? 'opacity-100 scale-100' : 'opacity-0 scale-95'}`}>
            
            {/* Card Container */}
            <div className="relative group">
              {/* Animated glow effect on hover */}
              <div className="absolute -inset-0.5 bg-gradient-to-r from-gray-200 via-gray-100 to-gray-200 rounded-3xl opacity-75 group-hover:opacity-100 transition-opacity duration-500 blur-sm" />
              
              <div className="relative bg-white rounded-3xl border border-gray-200 shadow-2xl shadow-gray-200/50 overflow-hidden">
                
                <div className="relative p-6 sm:p-8 lg:p-10">
                  {/* Step 1: Empresa Selection */}
                  {step === 'empresa' && (
                    <div className="space-y-6">
                      {/* Header */}
                      <div className="text-center">
                        {/* Logo/Icon mais elegante */}
                        <div className="relative w-20 h-20 mx-auto mb-5">
                          <div className="absolute inset-0 bg-gradient-to-br from-gray-100 to-gray-50 rounded-2xl rotate-6 transition-transform group-hover:rotate-12" />
                          <div className="absolute inset-0 bg-white rounded-2xl shadow-lg border border-gray-200 flex items-center justify-center">
                            <Building2 className="w-9 h-9 text-gray-700" />
                          </div>
                        </div>
                        <h2 className="text-2xl font-bold text-gray-800 mb-2">Acesso ao Sistema</h2>
                        <p className="text-gray-400 text-sm">Selecione a empresa para continuar</p>
                      </div>

                      {/* Company Cards */}
                      <div className="space-y-3">
                        {empresas.map((emp) => (
                          <button
                            key={emp.value}
                            type="button"
                            onClick={() => handleEmpresaClick(emp.value)}
                            onMouseEnter={() => setHoveredCard(emp.value)}
                            onMouseLeave={() => setHoveredCard(null)}
                            className={`relative w-full p-4 rounded-xl border-2 transition-all duration-300 overflow-hidden group
                              ${hoveredCard === emp.value 
                                ? 'border-gray-300 shadow-lg scale-[1.02] bg-gray-50' 
                                : 'border-gray-200 hover:border-gray-300 bg-white hover:bg-gray-50/50'
                              }
                            `}
                          >
                            <div className="relative flex items-center gap-4">
                              {/* Logo */}
                              <div className="w-20 h-20 rounded-xl bg-white p-2 shadow-sm group-hover:shadow-md transition-all duration-300 border border-gray-200">
                                <img src={emp.logo} alt={emp.label} className="w-full h-full object-contain" />
                              </div>
                              
                              {/* Info */}
                              <div className="flex-1 text-left">
                                <h3 className="font-semibold text-gray-800 text-base group-hover:text-gray-900 transition-colors">
                                  {emp.label}
                                </h3>
                                <p className="text-sm text-gray-400 group-hover:text-gray-500 transition-colors">
                                  {emp.subtitle}
                                </p>
                              </div>
                            </div>
                          </button>
                        ))}
                      </div>

                      {/* Footer - mais elegante */}
                      <div className="pt-5 mt-2">
                        <div className="flex items-center gap-3">
                          <div className="flex-1 h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />
                          <p className="text-[11px] text-gray-400 font-medium tracking-wide">
                            ACESSO RESTRITO
                          </p>
                          <div className="flex-1 h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />
                        </div>
                      </div>
                    </div>
                  )}

                  {/* Step 2: Credenciais */}
                  {step === 'credenciais' && (
                    <div className="space-y-5">
                      {/* Back + Selected Company */}
                      <div className="animate-fade-in-up" style={{ animationDelay: '0ms' }}>
                        <button
                          type="button"
                          onClick={handleVoltarEmpresa}
                          className="flex items-center gap-2 text-gray-400 hover:text-gray-700 transition-colors text-sm mb-3 group"
                        >
                          <ChevronLeft className="w-4 h-4 group-hover:-translate-x-1 transition-transform" />
                          <span>Trocar empresa</span>
                        </button>
                        
                        {/* Selected Company Card */}
                        <div className="flex items-center gap-3 p-3 rounded-xl bg-gradient-to-r from-gray-50 to-white border border-gray-200 shadow-sm">
                          <div className="w-14 h-14 rounded-lg bg-white p-1.5 shadow-sm border border-gray-200">
                            <img src={getEmpresaSelecionada()?.logo} alt="" className="w-full h-full object-contain" />
                          </div>
                          <div>
                            <h3 className="font-medium text-gray-800 text-sm">{getEmpresaSelecionada()?.label}</h3>
                            <p className="text-xs text-gray-500">{getEmpresaSelecionada()?.subtitle}</p>
                          </div>
                        </div>
                      </div>

                      {/* Login Form */}
                      <form onSubmit={handleSubmit} className="space-y-4">
                        {/* Usuário */}
                        <div className="animate-fade-in-up" style={{ animationDelay: '100ms' }}>
                          <label className="block text-xs font-semibold text-gray-500 uppercase tracking-wide mb-2">
                            Usuário
                          </label>
                          <div className="relative group/input">
                            <User className="absolute left-3.5 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400 group-focus-within/input:text-gray-500 transition-colors" />
                            <input
                              ref={usuarioInputRef}
                              type="text"
                              value={usuarioDisplay}
                              onChange={handleUsuarioChange}
                              autoComplete="username"
                              className="w-full pl-11 pr-4 py-3.5 bg-gray-50/50 border border-gray-200 rounded-xl text-gray-800 placeholder-gray-400 focus:border-gray-400 focus:ring-2 focus:ring-gray-100 focus:bg-white outline-none transition-all"
                              placeholder="Digite seu usuário"
                              disabled={isLoading}
                            />
                          </div>
                        </div>

                        {/* Senha */}
                        <div className="animate-fade-in-up" style={{ animationDelay: '200ms' }}>
                          <label className="block text-xs font-semibold text-gray-500 uppercase tracking-wide mb-2">
                            Senha
                          </label>
                          <div className="relative group/input">
                            <Lock className="absolute left-3.5 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400 group-focus-within/input:text-gray-500 transition-colors" />
                            <input
                              ref={senhaInputRef}
                              type={showPassword ? 'text' : 'password'}
                              value={senha}
                              onChange={(e) => setSenha(e.target.value)}
                              onKeyDown={handleCapsLock}
                              onKeyUp={handleCapsLock}
                              autoComplete="current-password"
                              className="w-full pl-11 pr-12 py-3.5 bg-gray-50/50 border border-gray-200 rounded-xl text-gray-800 placeholder-gray-400 focus:border-gray-400 focus:ring-2 focus:ring-gray-100 focus:bg-white outline-none transition-all"
                              placeholder="Digite sua senha"
                              disabled={isLoading}
                            />
                            <button
                              type="button"
                              onClick={() => setShowPassword(!showPassword)}
                              tabIndex={-1}
                              className="absolute right-3.5 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-500 transition-colors p-1 rounded-lg hover:bg-gray-100"
                            >
                              {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                            </button>
                          </div>
                        </div>

                        {/* Caps Lock Warning */}
                        {capsLockOn && senha && (
                          <div className="flex items-center gap-2 p-3 bg-amber-50 border border-amber-200 rounded-xl text-amber-600 text-sm animate-pulse">
                            <KeyRound className="h-4 w-4" />
                            <span>Caps Lock está ativado</span>
                          </div>
                        )}

                        {/* Error */}
                        {error && (
                          <div className="flex items-center gap-3 p-4 bg-red-50 border border-red-200 rounded-xl text-red-600 animate-shake">
                            <AlertCircle className="h-5 w-5 flex-shrink-0" />
                            <span className="text-sm">{error}</span>
                          </div>
                        )}

                        {/* Success */}
                        {loginSuccess && (
                          <div className="flex items-center gap-3 p-4 bg-emerald-50 border border-emerald-200 rounded-xl text-emerald-600">
                            <CheckCircle2 className="h-5 w-5" />
                            <span className="text-sm font-medium">Login realizado com sucesso!</span>
                          </div>
                        )}

                        {/* Submit Button - Estilo mais elegante e neutro */}
                        <div className="animate-fade-in-up pt-2" style={{ animationDelay: '400ms' }}>
                          <button
                            type="submit"
                            disabled={isLoading || !usuario || !senha}
                            className="relative w-full py-4 rounded-xl font-semibold text-white overflow-hidden group disabled:cursor-not-allowed disabled:opacity-40 transition-all duration-300 shadow-lg shadow-gray-400/30 hover:shadow-gray-500/40 hover:scale-[1.02] active:scale-[0.98]"
                          >
                            {/* Button gradient background - tons de cinza escuro elegante */}
                            <div className="absolute inset-0 bg-gradient-to-r from-gray-800 via-gray-700 to-gray-800 transition-all duration-300" />
                            <div className="absolute inset-0 bg-gradient-to-r from-gray-700 via-gray-600 to-gray-700 opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
                            
                            {/* Shine effect */}
                            <div className="absolute inset-0 opacity-0 group-hover:opacity-100">
                              <div className="absolute top-0 -left-full w-full h-full bg-gradient-to-r from-transparent via-white/10 to-transparent skew-x-12 group-hover:left-full transition-all duration-700" />
                            </div>
                            
                            {/* Button content */}
                            <span className="relative flex items-center justify-center gap-2">
                              {isLoading ? (
                                <>
                                  <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24">
                                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                                  </svg>
                                  <span>Autenticando...</span>
                                </>
                            ) : (
                              <>
                                <LogIn className="w-5 h-5" />
                                <span>Entrar no Sistema</span>
                              </>
                            )}
                            </span>
                          </button>
                        </div>
                      </form>

                      {/* Footer - estilo mais elegante */}
                      <div className="pt-5 animate-fade-in-up" style={{ animationDelay: '500ms' }}>
                        <div className="flex items-center gap-3">
                          <div className="flex-1 h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />
                          <div className="flex items-center gap-1.5 text-gray-400">
                            <Shield className="w-3 h-3" />
                            <p className="text-[10px] font-medium tracking-wider uppercase">
                              Conexão Segura
                            </p>
                          </div>
                          <div className="flex-1 h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />
                        </div>
                      </div>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* Mobile branding - only shows on mobile */}
            <div className="mt-6 text-center lg:hidden">
              <p className="text-gray-500 text-sm">
                © {COPYRIGHT_YEAR} {APP_NAME}
                <br />
                <span className="text-xs text-gray-400">v{APP_VERSION}</span>
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Custom animations */}
      <style>{`
        @keyframes blob {
          0%, 100% { 
            transform: translate(0, 0) scale(1); 
          }
          25% { 
            transform: translate(30px, -40px) scale(1.1); 
          }
          50% { 
            transform: translate(-30px, 30px) scale(0.9); 
          }
          75% { 
            transform: translate(40px, 15px) scale(1.05); 
          }
        }
        .animate-blob {
          animation: blob 12s ease-in-out infinite;
        }
        .animation-delay-2000 {
          animation-delay: 2s;
        }
        .animation-delay-3000 {
          animation-delay: 3s;
        }
        .animation-delay-4000 {
          animation-delay: 4s;
        }
        
        @keyframes spin-slow {
          from { transform: rotate(0deg); }
          to { transform: rotate(360deg); }
        }
        .animate-spin-slow {
          animation: spin-slow 20s linear infinite;
        }
        
        @keyframes float-up {
          0% { 
            transform: translateY(0) translateX(0);
            opacity: 0;
          }
          5% {
            opacity: 0.8;
          }
          95% {
            opacity: 0.8;
          }
          100% { 
            transform: translateY(-110vh) translateX(30px);
            opacity: 0;
          }
        }
        .animate-float-up {
          animation: float-up linear infinite;
        }
        
        @keyframes fade-in-up {
          0% {
            opacity: 0;
            transform: translateY(15px);
          }
          100% {
            opacity: 1;
            transform: translateY(0);
          }
        }
        .animate-fade-in-up {
          animation: fade-in-up 0.5s ease-out forwards;
          opacity: 0;
        }
        
        @keyframes shake {
          0%, 100% { transform: translateX(0); }
          10%, 30%, 50%, 70%, 90% { transform: translateX(-5px); }
          20%, 40%, 60%, 80% { transform: translateX(5px); }
        }
        .animate-shake {
          animation: shake 0.5s ease-in-out;
        }
      `}</style>
    </div>
  );
};

export default Login;
