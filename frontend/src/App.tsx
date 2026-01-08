import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Layout from './components/Layout/Layout';
import { LoginPage } from './pages/Login';
import { DashboardPage } from './pages/Dashboard';
import { ClassificacaoFiscalPage, ClassificacaoFiscalFormPage } from './pages/ClassificacaoFiscal';
import { ClassTribManagementPage } from './pages/ClassTrib';
import { UsuariosPage } from './pages/Usuarios';
import { LogsPage } from './pages/Logs';
import { GeralPage, GeralFormPage } from './pages/Geral';
import { ProdutosPage, ProdutoFormPage } from './pages/Produtos';
import MovimentoContabilPage from './pages/MovimentoContabil/MovimentoContabilPage';
import { NotaFiscalListPage, NotaFiscalFormPage } from './pages/NotaFiscal';
import { EmitentePage } from './pages/Emitente';
import { EntradaEstoquePage } from './pages/EntradaEstoque';
import { 
  VeiculosPage, VeiculoFormPage,
  ReboquesPage, ReboqueFormPage,
  ViagensPage, ViagemFormPage,
  ManutencoesPage, ManutencaoFormPage,
  MotoristasPage, MotoristaFormPage
} from './pages/Transporte';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          
          {/* Rotas protegidas com layout */}
          <Route
            element={
              <ProtectedRoute>
                <Layout />
              </ProtectedRoute>
            }
          >
            <Route path="/dashboard" element={<DashboardPage />} />
            
            {/* Comercial */}
            <Route path="/orcamentos" element={<div className="text-xl font-semibold text-gray-900">Orçamentos</div>} />
            <Route path="/pedidos" element={<div className="text-xl font-semibold text-gray-900">Pedidos</div>} />
            <Route path="/projetos" element={<div className="text-xl font-semibold text-gray-900">Projetos de Irrigação</div>} />
            <Route path="/licitacoes" element={<div className="text-xl font-semibold text-gray-900">Licitações</div>} />
            
            {/* Operacional */}
            <Route path="/ordens-servico" element={<div className="text-xl font-semibold text-gray-900">Ordens de Serviço</div>} />
            <Route path="/producao" element={<div className="text-xl font-semibold text-gray-900">Produção</div>} />
            <Route path="/estoque" element={<div className="text-xl font-semibold text-gray-900">Controle de Estoque</div>} />
            <Route path="/estoque/entrada" element={<EntradaEstoquePage />} />
            <Route path="/compras" element={<div className="text-xl font-semibold text-gray-900">Pedidos de Compra</div>} />
            
            {/* Transporte */}
            <Route path="/transporte/veiculos" element={<VeiculosPage />} />
            <Route path="/transporte/veiculos/novo" element={<VeiculoFormPage />} />
            <Route path="/transporte/veiculos/:id" element={<VeiculoFormPage />} />
            <Route path="/transporte/veiculos/:id/editar" element={<VeiculoFormPage />} />
            
            <Route path="/transporte/reboques" element={<ReboquesPage />} />
            <Route path="/transporte/reboques/novo" element={<ReboqueFormPage />} />
            <Route path="/transporte/reboques/:id" element={<ReboqueFormPage />} />
            <Route path="/transporte/reboques/:id/editar" element={<ReboqueFormPage />} />
            
            <Route path="/transporte/viagens" element={<ViagensPage />} />
            <Route path="/transporte/viagens/nova" element={<ViagemFormPage />} />
            <Route path="/transporte/viagens/:id" element={<ViagemFormPage />} />
            <Route path="/transporte/viagens/:id/editar" element={<ViagemFormPage />} />
            
            <Route path="/transporte/manutencoes" element={<ManutencoesPage />} />
            <Route path="/transporte/manutencoes/nova" element={<ManutencaoFormPage />} />
            <Route path="/transporte/manutencoes/:id" element={<ManutencaoFormPage />} />
            <Route path="/transporte/manutencoes/:id/editar" element={<ManutencaoFormPage />} />
            
            <Route path="/transporte/motoristas" element={<MotoristasPage />} />
            <Route path="/transporte/motoristas/novo" element={<MotoristaFormPage />} />
            <Route path="/transporte/motoristas/:id" element={<MotoristaFormPage />} />
            <Route path="/transporte/motoristas/:id/editar" element={<MotoristaFormPage />} />
            <Route path="/transporte/motoristas/:id/visualizar" element={<MotoristaFormPage />} />
            
            {/* Financeiro */}
            <Route path="/contas-pagar" element={<div className="text-xl font-semibold text-gray-900">Contas a Pagar</div>} />
            <Route path="/contas-receber" element={<div className="text-xl font-semibold text-gray-900">Contas a Receber</div>} />
            <Route path="/notas-fiscais" element={<Navigate to="/faturamento/notas-fiscais" replace />} />
            
            {/* Faturamento - Notas Fiscais */}
            <Route path="/faturamento/notas-fiscais" element={<NotaFiscalListPage />} />
            <Route path="/faturamento/notas-fiscais/nova" element={<NotaFiscalFormPage />} />
            <Route path="/faturamento/notas-fiscais/:id" element={<NotaFiscalFormPage />} />
            <Route path="/faturamento/notas-fiscais/:id/editar" element={<NotaFiscalFormPage />} />
            
            {/* Cadastros */}
            <Route path="/cadastros/geral" element={<GeralPage />} />
            <Route path="/cadastros/geral/novo" element={<GeralFormPage />} />
            <Route path="/cadastros/geral/:id" element={<GeralFormPage />} />
            <Route path="/cadastros/geral/:id/visualizar" element={<GeralFormPage />} />
            
            {/* Atalhos para filtros específicos */}
            <Route path="/cadastros/clientes" element={<GeralPage />} />
            <Route path="/cadastros/fornecedores" element={<GeralPage />} />
            <Route path="/cadastros/transportadoras" element={<GeralPage />} />
            <Route path="/cadastros/vendedores" element={<GeralPage />} />
            
            <Route path="/clientes" element={<Navigate to="/cadastros/clientes?tipo=cliente" replace />} />
            <Route path="/fornecedores" element={<Navigate to="/cadastros/fornecedores?tipo=fornecedor" replace />} />
            
            {/* Produtos */}
            <Route path="/cadastros/produtos" element={<ProdutosPage />} />
            <Route path="/cadastros/produtos/novo" element={<ProdutoFormPage />} />
            <Route path="/cadastros/produtos/:id" element={<ProdutoFormPage />} />
            <Route path="/cadastros/produtos/:id/editar" element={<ProdutoFormPage />} />
            <Route path="/produtos" element={<Navigate to="/cadastros/produtos" replace />} />
            
            {/* Movimento Contábil / Ajuste de Estoque */}
            <Route path="/estoque/movimento-contabil" element={<MovimentoContabilPage />} />
            <Route path="/estoque/inventario" element={<Navigate to="/estoque/movimento-contabil" replace />} />
            
            <Route path="/classificacao-fiscal" element={<ClassificacaoFiscalPage />} />
            <Route path="/classificacao-fiscal/:id" element={<ClassificacaoFiscalFormPage />} />
            <Route path="/classtrib" element={<ClassTribManagementPage />} />
            
            {/* Análises */}
            <Route path="/relatorios" element={<div className="text-xl font-semibold text-gray-900">Relatórios</div>} />
            
            {/* Sistema */}
            <Route path="/configuracoes" element={<div className="text-xl font-semibold text-gray-900">Configurações</div>} />
            <Route path="/emitente" element={<EmitentePage />} />
            <Route path="/perfil" element={<div className="text-xl font-semibold text-gray-900">Meu Perfil</div>} />
            <Route path="/usuarios" element={<UsuariosPage />} />
            <Route path="/permissoes" element={<Navigate to="/usuarios" replace />} />
            <Route path="/logs" element={<LogsPage />} />
          </Route>

          <Route path="/" element={<Navigate to="/dashboard" replace />} />
          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
