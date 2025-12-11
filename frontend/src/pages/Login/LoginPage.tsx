import React, { useState, useEffect, useRef, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import {
  User, Lock, AlertCircle, Eye, EyeOff, LogIn, CheckCircle2,
  KeyRound, ChevronLeft, Shield, Building2, Activity, ArrowRight
} from 'lucide-react';
import { APP_VERSION, APP_NAME, COPYRIGHT_YEAR } from '../../config/version';

const Login: React.FC = () => {
  const [empresa, setEmpresa] = useState('');
  const [usuario, setUsuario] = useState('');
  const [usuarioDisplay, setUsuarioDisplay] = useState('');
  const [senha, setSenha] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [mounted, setMounted] = useState(false);
  const [capsLockOn, setCapsLockOn] = useState(false);
  const [loginSuccess, setLoginSuccess] = useState(false);
  const [step, setStep] = useState<'empresa' | 'credenciais'>('empresa');
  const [hoveredCard, setHoveredCard] = useState<string | null>(null);
  const [isTransitioning, setIsTransitioning] = useState(false);

  const usuarioInputRef = useRef<HTMLInputElement>(null);
  const senhaInputRef = useRef<HTMLInputElement>(null);
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/dashboard', { replace: true });
    }
  }, [isAuthenticated, navigate]);

  // Particles - Mais visíveis
  const particles = useMemo(() =>
    [...Array(45)].map((_, i) => ({
      id: i,
      left: `${Math.random() * 100}%`,
      size: Math.random() * 4 + 3,
      duration: Math.random() * 15 + 18,
      delay: Math.random() * 15,
      color: Math.random() > 0.5 ? 'bg-blue-400' : 'bg-indigo-400'
    }))
    , []);

  const capitalizeWords = (text: string): string => {
    return text
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  };

  const handleUsuarioChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const rawValue = e.target.value;
    setUsuario(rawValue);
    setUsuarioDisplay(capitalizeWords(rawValue));
  };

  useEffect(() => {
    setMounted(true);
  }, []);

  const empresas = [
    {
      value: 'irrigacao',
      label: 'Irrigação Penápolis',
      subtitle: 'Sistema de Gestão Agrícola',
      logo: '/logos/logo_IP.png',
      color: 'text-emerald-600',
      bg: 'bg-emerald-50'
    },
    {
      value: 'chinellato',
      label: 'Chinellato Transportes',
      subtitle: 'Logística e Frotas',
      logo: '/logos/logo_chinellato.png',
      color: 'text-indigo-600',
      bg: 'bg-indigo-50'
    },
  ];

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);
    try {
      await login(empresa, usuario, senha);
      setLoginSuccess(true);
      setTimeout(() => navigate('/dashboard'), 800);
    } catch (err: any) {
      setError(err.message || 'Credenciais inválidas');
      setSenha('');
      setTimeout(() => senhaInputRef.current?.focus(), 100);
    } finally {
      setIsLoading(false);
    }
  };

  const handleEmpresaClick = (value: string) => {
    setEmpresa(value);
    setError('');
    setIsTransitioning(true);

    setTimeout(() => {
      setStep('credenciais');
      setIsTransitioning(false);
      setTimeout(() => usuarioInputRef.current?.focus(), 100);
    }, 400);
  };

  const handleVoltarEmpresa = () => {
    setIsTransitioning(true);

    setTimeout(() => {
      setStep('empresa');
      setUsuario('');
      setUsuarioDisplay('');
      setSenha('');
      setError('');
      setIsTransitioning(false);
    }, 400);
  };

  const getEmpresaSelecionada = () => empresas.find(emp => emp.value === empresa);

  const handleCapsLock = (e: React.KeyboardEvent) => setCapsLockOn(e.getModifierState('CapsLock'));

  return (
    <div className="min-h-screen w-full flex items-center justify-center bg-[#F8FAFC] relative overflow-hidden font-sans selection:bg-blue-500/20">

      {/* Modern Gradient Background */}
      <div className="absolute inset-0 bg-[radial-gradient(circle_at_top_left,_var(--tw-gradient-stops))] from-blue-100/30 via-transparent to-transparent opacity-60" />
      <div className="absolute inset-0 bg-[radial-gradient(circle_at_bottom_right,_var(--tw-gradient-stops))] from-indigo-100/30 via-transparent to-transparent opacity-60" />

      {/* Rising Particles - Mais visíveis */}
      {particles.map((p) => (
        <div
          key={p.id}
          className={`absolute rounded-full ${p.color}`}
          style={{
            left: p.left,
            bottom: '0',
            width: `${p.size}px`,
            height: `${p.size}px`,
            animation: `particleRise ${p.duration}s infinite ease-out`,
            animationDelay: `-${p.delay}s`,
            opacity: 0.7
          }}
        />
      ))}

      {/* Decorative Circles */}
      <div className="absolute top-[-10%] left-[-5%] w-[600px] h-[600px] rounded-full border border-slate-200/40 opacity-30 pointer-events-none" />
      <div className="absolute bottom-[-10%] right-[-5%] w-[500px] h-[500px] rounded-full border border-slate-200/40 opacity-30 pointer-events-none" />


      <div className={`w-full max-w-[1400px] mx-auto px-6 py-4 lg:px-12 flex flex-col lg:flex-row items-center justify-center gap-16 relative z-10 transition-all duration-1000 ${mounted ? 'opacity-100 translate-y-0' : 'opacity-0 translate-y-10'}`}>

        {/* Left Side - Text Content */}
        <div className="w-full lg:w-1/2 space-y-6 lg:pr-16 text-center lg:text-left">
          <div className="space-y-3">
            <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-white border border-slate-200 text-slate-600 text-[10px] font-bold tracking-widest uppercase shadow-sm mb-2">
              <span className="w-1.5 h-1.5 rounded-full bg-blue-500 animate-pulse" />
              Portal Corporativo
            </div>

            <h1 className="text-4xl lg:text-6xl font-bold text-slate-900 leading-[1.1] tracking-tight">
              Bem-vindo,
              <br />
              <span className="text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-indigo-600">
                colaborador.
              </span>
            </h1>

            <p className="text-sm lg:text-base text-slate-500 max-w-md mx-auto lg:mx-0 leading-relaxed font-medium">
              Acesse sua conta para continuar trabalhando.
              <br className="hidden lg:block" />
              Selecione a empresa e insira suas credenciais.
            </p>
          </div>

          <div className="hidden lg:flex items-center gap-8 pt-6 border-t border-slate-200/60">
            <div className="flex items-center gap-3 group cursor-default">
              <div className="w-10 h-10 rounded-xl bg-white shadow-sm border border-slate-100 flex items-center justify-center text-blue-600 group-hover:scale-105 transition-transform duration-300">
                <Shield className="w-5 h-5" />
              </div>
              <div>
                <p className="text-sm font-bold text-slate-900">Acesso Seguro</p>
                <p className="text-[11px] text-slate-500 font-medium">Criptografia ponta a ponta</p>
              </div>
            </div>
            <div className="flex items-center gap-3 group cursor-default">
              <div className="w-10 h-10 rounded-xl bg-white shadow-sm border border-slate-100 flex items-center justify-center text-emerald-600 group-hover:scale-105 transition-transform duration-300">
                <Activity className="w-5 h-5" />
              </div>
              <div>
                <p className="text-sm font-bold text-slate-900">Sistema Online</p>
                <p className="text-[11px] text-slate-500 font-medium">Serviços operacionais</p>
              </div>
            </div>
          </div>
        </div>

        {/* Right Side - Floating Card */}
        <div className="w-full lg:w-[450px] flex-shrink-0">
          <div className="bg-white/80 backdrop-blur-xl rounded-[2rem] shadow-[0_20px_40px_-12px_rgba(0,0,0,0.05)] p-8 md:p-10 border border-white/50 relative overflow-hidden">

            {/* Top Icon - Apenas na seleção de empresa */}
            {step === 'empresa' && (
              <div className="w-16 h-16 mx-auto mb-6 rounded-2xl bg-gradient-to-br from-slate-50 to-slate-100 border border-slate-200/60 flex items-center justify-center text-slate-700 shadow-inner group animate-in fade-in slide-in-from-top-2 duration-500">
                <Building2 className="w-8 h-8 group-hover:scale-110 transition-transform duration-300" />
              </div>
            )}

            {/* Título - Apenas na seleção de empresa */}
            {step === 'empresa' && (
              <div className="text-center mb-8 animate-in fade-in slide-in-from-top-3 duration-500">
                <h2 className="text-2xl font-bold text-slate-900 tracking-tight">Acesso ao Sistema</h2>
                <p className="text-sm text-slate-500 mt-1.5 font-medium">
                  Selecione a empresa para continuar
                </p>
              </div>
            )}

            {step === 'empresa' ? (
              <div className={`space-y-4 transition-all duration-500 ${isTransitioning ? 'opacity-0 translate-y-4' : 'opacity-100 translate-y-0'}`}>
                {empresas.map((emp, index) => (
                  <button
                    key={emp.value}
                    onClick={() => handleEmpresaClick(emp.value)}
                    onMouseEnter={() => setHoveredCard(emp.value)}
                    onMouseLeave={() => setHoveredCard(null)}
                    disabled={isTransitioning}
                    style={{
                      animationDelay: `${index * 100}ms`
                    }}
                    className={`
                      w-full p-4 rounded-xl border transition-all duration-300 flex items-center gap-4 group relative overflow-hidden
                      animate-in fade-in slide-in-from-bottom-4
                      disabled:cursor-not-allowed disabled:opacity-60
                      ${hoveredCard === emp.value && !isTransitioning
                        ? 'border-blue-500/30 bg-blue-50/40 shadow-md scale-[1.02] -translate-y-1'
                        : 'border-slate-100 bg-white hover:border-slate-200 hover:bg-slate-50/50 hover:shadow-sm'
                      }
                    `}
                  >
                    <div className="w-12 h-12 rounded-lg bg-white p-1.5 shadow-sm border border-slate-100 flex-shrink-0 flex items-center justify-center transition-transform duration-300 group-hover:scale-105">
                      <img src={emp.logo} alt={emp.label} className="w-full h-full object-contain transition-all duration-300" />
                    </div>
                    <div className="flex-1 min-w-0 z-10 text-left">
                      <h3 className={`font-bold text-sm transition-all duration-300 ${hoveredCard === emp.value && !isTransitioning ? 'text-blue-700 translate-x-1' : 'text-slate-900'}`}>
                        {emp.label}
                      </h3>
                      <p className="text-xs text-slate-500 truncate mt-0.5 transition-all duration-300">{emp.subtitle}</p>
                    </div>
                    <div className={`
                      w-6 h-6 rounded-full flex items-center justify-center transition-all duration-300 z-10
                      ${hoveredCard === emp.value && !isTransitioning ? 'bg-blue-500 text-white translate-x-0 opacity-100 scale-100' : 'bg-transparent text-transparent translate-x-2 opacity-0 scale-0'}
                    `}>
                      <ArrowRight className="w-3 h-3" />
                    </div>
                  </button>
                ))}
              </div>
            ) : (
              <div className={`space-y-6 transition-all duration-500 ${isTransitioning ? 'opacity-0 translate-y-4' : 'opacity-100 translate-y-0'} ${step === 'credenciais' ? 'mt-6' : ''}`}>
                <button
                  onClick={handleVoltarEmpresa}
                  disabled={isTransitioning}
                  className="flex items-center gap-1.5 text-[10px] font-bold text-slate-400 hover:text-blue-600 transition-all duration-300 mx-auto uppercase tracking-wider py-1.5 px-3 rounded-full hover:bg-blue-50 hover:scale-105 animate-in fade-in slide-in-from-top-2 disabled:cursor-not-allowed disabled:opacity-60"
                >
                  <ChevronLeft className="w-3 h-3 transition-transform duration-300 group-hover:-translate-x-1" />
                  Voltar
                </button>

                <div className="flex items-center gap-4 p-4 rounded-xl bg-slate-50/50 border border-slate-100 animate-in fade-in slide-in-from-top-3 duration-300 transition-all hover:shadow-sm">
                  <div className="w-14 h-14 rounded-lg bg-white p-2 shadow-sm border border-slate-100 transition-transform duration-300 hover:scale-105">
                    <img src={getEmpresaSelecionada()?.logo} alt="" className="w-full h-full object-contain" />
                  </div>
                  <div>
                    <h3 className="font-bold text-base text-slate-900">{getEmpresaSelecionada()?.label}</h3>
                    <p className="text-xs text-slate-500 mt-0.5">{getEmpresaSelecionada()?.subtitle}</p>
                  </div>
                </div>

                <form onSubmit={handleSubmit} className="space-y-4">
                  <div className="space-y-1.5 animate-in fade-in slide-in-from-right-4 duration-300" style={{ animationDelay: '100ms' }}>
                    <label className="text-[10px] font-bold text-slate-500 uppercase tracking-wider ml-1">Usuário</label>
                    <div className="relative group">
                      <div className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 group-focus-within:text-blue-600 transition-all duration-300 group-focus-within:scale-110">
                        <User className="w-4 h-4" />
                      </div>
                      <input
                        ref={usuarioInputRef}
                        type="text"
                        value={usuarioDisplay}
                        onChange={handleUsuarioChange}
                        className="w-full pl-10 pr-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 placeholder:text-slate-400 focus:ring-2 focus:ring-blue-500/10 focus:border-blue-500 outline-none transition-all duration-300 font-medium text-sm hover:bg-slate-100/50 focus:bg-white focus:shadow-sm"
                        placeholder="Seu usuário"
                        disabled={isLoading}
                      />
                    </div>
                  </div>

                  <div className="space-y-1.5 animate-in fade-in slide-in-from-right-4 duration-300" style={{ animationDelay: '200ms' }}>
                    <label className="text-[10px] font-bold text-slate-500 uppercase tracking-wider ml-1">Senha</label>
                    <div className="relative group">
                      <div className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 group-focus-within:text-blue-600 transition-all duration-300 group-focus-within:scale-110">
                        <Lock className="w-4 h-4" />
                      </div>
                      <input
                        ref={senhaInputRef}
                        type={showPassword ? 'text' : 'password'}
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                        onKeyDown={handleCapsLock}
                        onKeyUp={handleCapsLock}
                        className="w-full pl-10 pr-10 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 placeholder:text-slate-400 focus:ring-2 focus:ring-blue-500/10 focus:border-blue-500 outline-none transition-all duration-300 font-medium text-sm hover:bg-slate-100/50 focus:bg-white focus:shadow-sm"
                        placeholder="Sua senha"
                        disabled={isLoading}
                      />
                      <button
                        type="button"
                        onClick={() => setShowPassword(!showPassword)}
                        className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 transition-all duration-300 p-1.5 hover:bg-slate-100 rounded-lg hover:scale-110"
                      >
                        {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>
                  </div>

                  {capsLockOn && senha && (
                    <div className="flex items-center gap-2 p-2.5 bg-amber-50 border border-amber-100 rounded-lg text-amber-600 text-xs font-medium animate-in fade-in slide-in-from-top-2 duration-300">
                      <KeyRound className="w-3.5 h-3.5 animate-pulse" />
                      <span>Caps Lock ativado</span>
                    </div>
                  )}

                  {error && (
                    <div className="flex items-center gap-2 p-3 bg-red-50 border border-red-100 rounded-lg text-red-600 text-xs font-medium animate-in fade-in slide-in-from-top-2 duration-300">
                      <AlertCircle className="w-4 h-4 flex-shrink-0 animate-pulse" />
                      <span>{error}</span>
                    </div>
                  )}

                  {loginSuccess && (
                    <div className="flex items-center gap-2 p-3 bg-emerald-50 border border-emerald-100 rounded-lg text-emerald-600 text-xs font-medium animate-in fade-in slide-in-from-top-2 duration-300">
                      <CheckCircle2 className="w-4 h-4 animate-pulse" />
                      <span>Login realizado com sucesso!</span>
                    </div>
                  )}

                  <button
                    type="submit"
                    disabled={isLoading || !usuario || !senha}
                    className="w-full py-3.5 rounded-xl font-bold text-white text-sm bg-slate-900 hover:bg-slate-800 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-300 shadow-lg shadow-slate-900/20 hover:shadow-slate-900/30 active:scale-[0.98] flex items-center justify-center gap-2 mt-2 hover:-translate-y-0.5 animate-in fade-in slide-in-from-bottom-4"
                    style={{ animationDelay: '300ms' }}
                  >
                    {isLoading ? (
                      <>
                        <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                        <span>Entrando...</span>
                      </>
                    ) : (
                      <>
                        <LogIn className="w-4 h-4 transition-transform duration-300 group-hover:translate-x-1" />
                        <span>Acessar Sistema</span>
                      </>
                    )}
                  </button>
                </form>
              </div>
            )}

            <div className="mt-8 pt-6 border-t border-slate-100 text-center">
              <p className="text-[10px] text-slate-400 font-bold tracking-widest uppercase">
                Acesso Restrito • {APP_NAME}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Footer Version */}
      <div className="absolute bottom-6 left-8 hidden lg:block">
        <p className="text-[10px] text-slate-400 font-medium">
          © {COPYRIGHT_YEAR} Sistema Empresarial
          <span className="mx-2">•</span>
          Versão {APP_VERSION}
        </p>
      </div>
    </div>
  );
};

export default Login;
