import { useEffect, useState } from 'react';
import {
  Search,
  Filter,
  Download,
  AlertCircle,
  CheckCircle,
  BarChart3,
  Eye,
  X,
  Printer,
  FileText,
  Percent,
  Tag,
  BookOpen,
  XCircle
} from 'lucide-react';
import { classTribService } from '../../services/Fiscal/classTribService';
import type { ClassTribDto } from '../../services/Fiscal/classTribService';
import { usePermissaoTela } from '../../hooks/usePermissaoTela';

// Componentes reutilizáveis
import {
  AlertaErro,
  DataTable,
  CabecalhoPagina,
  type ColumnConfig,
  EstadoCarregando,

} from '../../components/common';

interface FilterState {
  csts: string[];
  tipoAliquota: string;
  minReducaoIBS: number | null;
  maxReducaoIBS: number | null;
  minReducaoCBS: number | null;
  maxReducaoCBS: number | null;
  validoNFe: boolean | null;
  tributacaoRegular: boolean | null;
  creditoPresumido: boolean | null;
  descricao: string;
  ordenarPor: string;
}

interface Stats {
  totalClassificacoes: number;
  totalValidoNFe: number;
  mediaReducaoIBS: number;
  mediaReducaoCBS: number;
  totalComReducaoIBS: number;
  totalComReducaoCBS: number;
}

export default function ClassTribManagementPage() {
  // Permissões da tela
  const { podeConsultar, carregando: carregandoPermissoes } = usePermissaoTela('ClassTrib');

  // Estados
  const [classTribs, setClassTribs] = useState<ClassTribDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [page, setPage] = useState(1);
  const [pageSize] = useState(50);
  const [total, setTotal] = useState(0);

  const [filter, setFilter] = useState<FilterState>({
    csts: [],
    tipoAliquota: '',
    minReducaoIBS: null,
    maxReducaoIBS: null,
    minReducaoCBS: null,
    maxReducaoCBS: null,
    validoNFe: null,
    tributacaoRegular: null,
    creditoPresumido: null,
    descricao: '',
    ordenarPor: 'codigo',
  });

  const [stats, setStats] = useState<Stats | null>(null);
  const [cstOptions, setCstOptions] = useState<Array<{ codigo: string; descricao: string; total: number }>>([]);
  const [tipoAliquotaOptions, setTipoAliquotaOptions] = useState<string[]>([]);
  const [showFilters, setShowFilters] = useState(false);

  // Estado para modal de visualização
  const [classTribSelecionado, setClassTribSelecionado] = useState<ClassTribDto | null>(null);
  const [showModal, setShowModal] = useState(false);

  // ============================================================================
  // FUNÇÕES
  // ============================================================================

  const abrirDetalhes = (classTrib: ClassTribDto) => {
    setClassTribSelecionado(classTrib);
    setShowModal(true);
  };

  const fecharModal = () => {
    setShowModal(false);
    setClassTribSelecionado(null);
  };

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const cstParams = filter.csts.length > 0 ? filter.csts.join(',') : undefined;

      // Nota: O serviço original pode precisar de adaptação para suportar todos os filtros no backend se não suportar
      // Aqui estamos mantendo a chamada original
      const response = await classTribService.listar(page, pageSize, cstParams);
      setClassTribs(response.items);
      setTotal(response.totalCount);
    } catch (err: any) {
      setError(err.response?.data?.error || 'Erro ao carregar dados');
    } finally {
      setLoading(false);
    }
  };

  const loadFiltersAndStats = async () => {
    try {
      const [csts, tipos, stats] = await Promise.all([
        classTribService.getCsts(),
        classTribService.getTiposAliquota(),
        classTribService.getEstatisticas(),
      ]);

      setCstOptions(csts);
      setTipoAliquotaOptions(tipos);
      setStats(stats);
    } catch (err) {
      console.error('Erro ao carregar filtros e estatísticas:', err);
    }
  };

  const handleFilterChange = (newFilter: Partial<FilterState>) => {
    setFilter(prev => ({ ...prev, ...newFilter }));
    setPage(1);
  };

  const resetFilters = () => {
    setFilter({
      csts: [],
      tipoAliquota: '',
      minReducaoIBS: null,
      maxReducaoIBS: null,
      minReducaoCBS: null,
      maxReducaoCBS: null,
      validoNFe: null,
      tributacaoRegular: null,
      creditoPresumido: null,
      descricao: '',
      ordenarPor: 'codigo',
    });
    setPage(1);
  };

  const handleExport = () => {
    const csv = [
      ['Código', 'CST', 'Descrição', 'Redução IBS', 'Redução CBS', 'Válido NFe'],
      ...classTribs.map(ct => [
        ct.codigoClassTrib,
        ct.codigoSituacaoTributaria,
        ct.descricaoClassTrib,
        ct.percentualReducaoIBS,
        ct.percentualReducaoCBS,
        ct.validoParaNFe ? 'Sim' : 'Não',
      ]),
    ]
      .map(row => row.map(cell => `"${cell}"`).join(','))
      .join('\n');

    const blob = new Blob([csv], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `classtrib-${new Date().toISOString().split('T')[0]}.csv`;
    a.click();
  };

  const handlePrint = () => {
    const printWindow = window.open('', '_blank');
    if (!printWindow) return;

    const content = `
      <html>
        <head>
          <title>Relatório de Classificações Tributárias</title>
          <style>
            @page { size: landscape; margin: 1cm; }
            body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 20px; color: #333; }
            .header { display: flex; justify-content: space-between; align-items: center; border-bottom: 2px solid #3b82f6; padding-bottom: 10px; margin-bottom: 20px; }
            .header h1 { margin: 0; color: #1e40af; font-size: 24px; }
            .header-info { text-align: right; font-size: 12px; color: #666; }
            table { width: 100%; border-collapse: collapse; margin-top: 10px; }
            th, td { border: 1px solid #e5e7eb; padding: 10px; text-align: left; font-size: 11px; }
            th { background-color: #f8fafc; color: #475569; font-weight: bold; text-transform: uppercase; }
            tr:nth-child(even) { background-color: #f1f5f9; }
            .badge { padding: 2px 6px; border-radius: 4px; font-weight: bold; font-size: 10px; }
            .badge-blue { background-color: #dbeafe; color: #1e40af; }
            .badge-green { background-color: #dcfce7; color: #166534; }
            .badge-gray { background-color: #f1f5f9; color: #475569; }
            .text-right { text-align: right; }
            .footer { margin-top: 30px; border-top: 1px solid #e5e7eb; padding-top: 10px; font-size: 10px; color: #94a3b8; text-align: center; }
          </style>
        </head>
        <body>
          <div class="header">
            <div>
              <h1>Relatório de Classificações Tributárias</h1>
              <p style="margin: 5px 0 0 0; font-size: 14px; color: #64748b;">Gestão de ClassTrib - Reforma Tributária</p>
            </div>
            <div class="header-info">
              <strong>Data de Emissão:</strong> ${new Date().toLocaleString()}<br>
              <strong>Total de Registros:</strong> ${classTribs.length} de ${total || stats?.totalClassificacoes || classTribs.length}
            </div>
          </div>

          <table>
            <thead>
              <tr>
                <th width="80">Código</th>
                <th width="60">CST</th>
                <th>Descrição</th>
                <th width="100" class="text-right">Red. IBS (%)</th>
                <th width="100" class="text-right">Red. CBS (%)</th>
                <th width="120">Tipo Alíquota</th>
                <th width="80">Válido NFe</th>
              </tr>
            </thead>
            <tbody>
              ${classTribs.map(ct => `
                <tr>
                  <td><span class="badge badge-blue">#${ct.codigoClassTrib}</span></td>
                  <td><strong>${ct.codigoSituacaoTributaria}</strong></td>
                  <td>${ct.descricaoClassTrib}</td>
                  <td class="text-right" style="color: #059669; font-weight: bold;">${ct.percentualReducaoIBS.toFixed(2)}%</td>
                  <td class="text-right" style="color: #059669; font-weight: bold;">${ct.percentualReducaoCBS.toFixed(2)}%</td>
                  <td>${ct.tipoAliquota || '-'}</td>
                  <td>
                    <span class="badge ${ct.validoParaNFe ? 'badge-green' : 'badge-gray'}">
                      ${ct.validoParaNFe ? 'Sim' : 'Não'}
                    </span>
                  </td>
                </tr>
              `).join('')}
            </tbody>
          </table>

          <div class="footer">
            Sistema de Gestão Empresarial - Relatório Gerado em ${new Date().toLocaleDateString()} às ${new Date().toLocaleTimeString()}
          </div>

          <script>
            window.onload = () => {
              setTimeout(() => {
                window.print();
                // window.close(); // Opcional: fechar após imprimir
              }, 500);
            };
          </script>
        </body>
      </html>
    `;

    printWindow.document.write(content);
    printWindow.document.close();
  };

  const handlePrintSingle = (ct: ClassTribDto) => {
    const printWindow = window.open('', '_blank');
    if (!printWindow) return;

    const content = `
      <html>
        <head>
          <title>Detalhes da Classificação Tributária - #${ct.codigoClassTrib}</title>
          <style>
            body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 40px; color: #333; }
            .card { border: 2px solid #e5e7eb; border-radius: 12px; padding: 30px; max-width: 800px; margin: 0 auto; }
            .header { border-bottom: 2px solid #3b82f6; padding-bottom: 15px; margin-bottom: 25px; display: flex; justify-content: space-between; align-items: center; }
            .header h1 { margin: 0; color: #1e40af; font-size: 22px; }
            .section { margin-bottom: 25px; }
            .section-title { font-size: 12px; font-weight: bold; color: #64748b; text-transform: uppercase; margin-bottom: 10px; border-bottom: 1px solid #f1f5f9; padding-bottom: 5px; }
            .grid { display: grid; grid-template-cols: 1fr 1fr; gap: 20px; }
            .label { font-size: 11px; color: #94a3b8; margin-bottom: 4px; }
            .value { font-size: 16px; font-weight: bold; color: #1e293b; }
            .badge { display: inline-block; padding: 4px 10px; border-radius: 6px; font-size: 12px; font-weight: bold; }
            .badge-blue { background-color: #dbeafe; color: #1e40af; }
            .footer { margin-top: 40px; text-align: center; font-size: 10px; color: #94a3b8; }
          </style>
        </head>
        <body>
          <div class="card">
            <div class="header">
              <h1>Detalhes da Classificação Tributária</h1>
              <span class="badge badge-blue">#${ct.codigoClassTrib}</span>
            </div>

            <div class="section">
              <div class="section-title">Informações Gerais</div>
              <div class="grid">
                <div>
                  <div class="label">Código ClassTrib</div>
                  <div class="value">${ct.codigoClassTrib}</div>
                </div>
                <div>
                  <div class="label">CST</div>
                  <div class="value">${ct.codigoSituacaoTributaria}</div>
                </div>
              </div>
              <div style="margin-top: 15px;">
                <div class="label">Descrição</div>
                <div class="value">${ct.descricaoClassTrib}</div>
              </div>
            </div>

            <div class="section">
              <div class="section-title">Percentuais de Redução</div>
              <div class="grid">
                <div style="background: #f0fdf4; padding: 15px; border-radius: 8px;">
                  <div class="label" style="color: #166534;">Redução IBS</div>
                  <div class="value" style="color: #15803d; font-size: 24px;">${ct.percentualReducaoIBS.toFixed(2)}%</div>
                </div>
                <div style="background: #f0fdf4; padding: 15px; border-radius: 8px;">
                  <div class="label" style="color: #166534;">Redução CBS</div>
                  <div class="value" style="color: #15803d; font-size: 24px;">${ct.percentualReducaoCBS.toFixed(2)}%</div>
                </div>
              </div>
            </div>

            <div class="section">
              <div class="section-title">Status e Regras</div>
              <div class="grid">
                <div>
                  <div class="label">Válido para NFe</div>
                  <div class="value">${ct.validoParaNFe ? 'Sim' : 'Não'}</div>
                </div>
                <div>
                  <div class="label">Tributação Regular</div>
                  <div class="value">${ct.tributacaoRegular ? 'Sim' : 'Não'}</div>
                </div>
                <div>
                  <div class="label">Tipo de Alíquota</div>
                  <div class="value">${ct.tipoAliquota || 'Não informado'}</div>
                </div>
                <div>
                  <div class="label">Crédito Presumido</div>
                  <div class="value">${ct.creditoPresumidoOperacoes ? 'Sim' : 'Não'}</div>
                </div>
              </div>
            </div>

            <div class="footer">
              Relatório gerado em ${new Date().toLocaleString()}
            </div>
          </div>
          <script>
            window.onload = () => {
              setTimeout(() => {
                window.print();
              }, 500);
            };
          </script>
        </body>
      </html>
    `;

    printWindow.document.write(content);
    printWindow.document.close();
  };

  // ============================================================================
  // EFFECTS
  // ============================================================================

  useEffect(() => {
    loadFiltersAndStats();
  }, []);

  useEffect(() => {
    loadData();
  }, [page, pageSize, filter]); // filter dependency might need debounce in real app

  // ============================================================================
  // COLUMNS CONFIG
  // ============================================================================
  const columns: ColumnConfig<ClassTribDto>[] = [
    {
      key: 'codigoClassTrib',
      header: 'Código',
      width: '100px',
      sortable: true,
      filterable: true,
      searchPlaceholder: 'Buscar código...',
      render: (item) => (
        <span className="font-mono font-bold text-blue-600 bg-blue-50 px-2 py-1 rounded-lg text-xs">
          #{item.codigoClassTrib}
        </span>
      )
    },
    {
      key: 'codigoSituacaoTributaria',
      header: 'CST',
      width: '100px',
      render: (item) => (
        <span className="inline-flex items-center px-2.5 py-1 bg-blue-50 text-blue-700 text-xs font-bold rounded-lg border border-blue-100">
          {item.codigoSituacaoTributaria}
        </span>
      )
    },
    {
      key: 'descricaoClassTrib',
      header: 'Descrição',
      render: (item) => (
        <span className="text-sm font-bold text-gray-900 line-clamp-1 group-hover:text-blue-600 transition-colors" title={item.descricaoClassTrib}>
          {item.descricaoClassTrib}
        </span>
      )
    },
    {
      key: 'percentualReducaoIBS',
      header: 'Red. IBS',
      align: 'center',
      width: '120px',
      render: (item) => (
        <span className="text-sm font-bold text-emerald-600">
          {item.percentualReducaoIBS.toFixed(2)}%
        </span>
      )
    },
    {
      key: 'percentualReducaoCBS',
      header: 'Red. CBS',
      align: 'center',
      width: '120px',
      render: (item) => (
        <span className="text-sm font-bold text-emerald-600">
          {item.percentualReducaoCBS.toFixed(2)}%
        </span>
      )
    },
    {
      key: 'tipoAliquota',
      header: 'Tipo Alíq.',
      align: 'center',
      width: '140px',
      render: (item) => (
        <span className="text-xs text-gray-500 font-bold uppercase tracking-wider">
          {item.tipoAliquota || '-'}
        </span>
      )
    },
    {
      key: 'validoParaNFe',
      header: 'NFe',
      align: 'center',
      width: '100px',
      render: (item) => (
        item.validoParaNFe ? (
          <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-bold bg-emerald-50 text-emerald-700 border border-emerald-100">
            <CheckCircle className="h-3.5 w-3.5" />
            Sim
          </span>
        ) : (
          <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-bold bg-gray-50 text-gray-400 border border-gray-100">
            <XCircle className="h-3.5 w-3.5" />
            Não
          </span>
        )
      )
    }
  ];

  // ============================================================================
  // RENDER
  // ============================================================================

  if (carregandoPermissoes) {
    return <EstadoCarregando mensagem="Verificando permissões..." />;
  }

  if (!podeConsultar) {
    return (
      <div className="min-h-screen bg-background flex items-center justify-center">
        <div className="text-center bg-surface p-8 rounded-xl shadow-lg border border-border max-w-md">
          <div className="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center mx-auto mb-4">
            <AlertCircle className="h-8 w-8 text-red-500" />
          </div>
          <h2 className="text-xl font-bold text-primary mb-2">Acesso Negado</h2>
          <p className="text-muted-foreground">Você não tem permissão para acessar esta tela.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6 pb-8">
      <CabecalhoPagina
        titulo="Gestão de ClassTrib"
        subtitulo="Gerenciar classificações tributárias da Reforma Tributária IBS/CBS"
        icone={BookOpen}
        acoes={
          <div className="flex items-center gap-2">
            <button
              onClick={handlePrint}
              className="inline-flex items-center justify-center gap-2 px-4 py-2 bg-surface text-primary border border-border rounded-xl hover:bg-surface-hover transition-all shadow-sm"
            >
              <Printer className="h-4 w-4" />
              <span className="hidden sm:inline">Imprimir</span>
            </button>
            <button
              disabled={true}
              className="inline-flex items-center justify-center gap-2 px-4 py-2 bg-surface-muted text-muted-foreground rounded-xl cursor-not-allowed opacity-60 border border-border"
              title="Dados já sincronizados"
            >
              <CheckCircle className="h-4 w-4" />
              <span className="hidden sm:inline">Sincronizado</span>
            </button>
          </div>
        }
      >
        {/* Filtros Rápidos */}
        <div className="flex flex-wrap gap-3">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className={`px-4 py-2 rounded-xl text-sm font-bold transition-all border flex items-center gap-2 ${showFilters
              ? 'bg-primary text-white border-primary shadow-md'
              : 'bg-surface text-muted-foreground border-border hover:bg-surface-hover hover:text-primary'
              }`}
          >
            <Filter className="w-4 h-4" />
            Filtros Avançados
            {(filter.csts.length > 0 || filter.descricao || filter.tipoAliquota) && (
              <span className="w-2 h-2 bg-secondary rounded-full animate-pulse"></span>
            )}
          </button>

          <button
            onClick={handlePrint}
            className="px-4 py-2 bg-blue-600 text-white rounded-xl text-sm font-bold hover:bg-blue-700 transition-all shadow-lg shadow-blue-600/20 flex items-center gap-2"
          >
            <Printer className="w-4 h-4" />
            Imprimir Relatório
          </button>

          {showFilters && (
            <div className="w-full mt-4 p-4 bg-surface border border-border rounded-xl shadow-sm animate-slide-up">
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                {/* Busca por Descrição */}
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">Descrição / Código</label>
                  <div className="relative">
                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
                    <input
                      type="text"
                      value={filter.descricao}
                      onChange={(e) => handleFilterChange({ descricao: e.target.value })}
                      placeholder="Buscar..."
                      className="w-full pl-9 pr-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                    />
                  </div>
                </div>

                {/* CSTs */}
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">CST</label>
                  <select
                    multiple
                    value={filter.csts}
                    onChange={(e) =>
                      handleFilterChange({
                        csts: Array.from(e.target.selectedOptions, (option) => option.value),
                      })
                    }
                    className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all h-24"
                  >
                    {cstOptions.map((cst) => (
                      <option key={cst.codigo} value={cst.codigo}>
                        {cst.codigo} - {cst.descricao}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Tipo de Alíquota */}
                <div className="space-y-1">
                  <label className="text-xs font-medium text-muted-foreground">Tipo de Alíquota</label>
                  <select
                    value={filter.tipoAliquota}
                    onChange={(e) => handleFilterChange({ tipoAliquota: e.target.value })}
                    className="w-full px-3 py-2 bg-surface-muted border border-border rounded-lg text-sm focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                  >
                    <option value="">Todos</option>
                    {tipoAliquotaOptions.map((tipo) => (
                      <option key={tipo} value={tipo}>
                        {tipo}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Botões de Ação do Filtro */}
                <div className="sm:col-span-2 lg:col-span-3 flex items-center gap-3 pt-2 border-t border-border mt-2">
                  <button
                    onClick={resetFilters}
                    className="px-4 py-2 text-sm font-medium text-red-600 bg-red-50 hover:bg-red-100 rounded-lg transition-colors flex items-center gap-2"
                  >
                    <XCircle className="w-4 h-4" />
                    Limpar Filtros
                  </button>
                  <button
                    onClick={handleExport}
                    className="px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 hover:bg-blue-100 rounded-lg transition-colors flex items-center gap-2 ml-auto"
                  >
                    <Download className="w-4 h-4" />
                    Exportar CSV
                  </button>
                  <button
                    onClick={handlePrint}
                    className="px-4 py-2 text-sm font-medium text-gray-600 bg-gray-50 hover:bg-gray-100 rounded-lg transition-colors flex items-center gap-2"
                  >
                    <Printer className="w-4 h-4" />
                    Imprimir Relatório
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
      </CabecalhoPagina>

      <div className="px-6 space-y-6">
        {/* Estatísticas */}
        {stats && (
          <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-xs text-muted-foreground font-medium uppercase">Total</p>
                  <p className="text-2xl font-bold text-primary mt-1">{stats.totalClassificacoes}</p>
                </div>
                <div className="p-3 bg-blue-50 rounded-xl">
                  <BarChart3 className="h-6 w-6 text-blue-600" />
                </div>
              </div>
            </div>

            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-xs text-muted-foreground font-medium uppercase">Válidas NFe</p>
                  <p className="text-2xl font-bold text-emerald-600 mt-1">{stats.totalValidoNFe}</p>
                </div>
                <div className="p-3 bg-emerald-50 rounded-xl">
                  <CheckCircle className="h-6 w-6 text-emerald-600" />
                </div>
              </div>
            </div>

            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div>
                <p className="text-xs text-muted-foreground font-medium uppercase">Média Red. IBS</p>
                <div className="flex items-baseline gap-2 mt-1">
                  <p className="text-2xl font-bold text-primary">{stats.mediaReducaoIBS.toFixed(2)}%</p>
                  <span className="text-xs text-muted-foreground">({stats.totalComReducaoIBS} itens)</span>
                </div>
              </div>
            </div>

            <div className="bg-surface rounded-xl border border-border p-4 shadow-sm hover:shadow-md transition-shadow">
              <div>
                <p className="text-xs text-muted-foreground font-medium uppercase">Média Red. CBS</p>
                <div className="flex items-baseline gap-2 mt-1">
                  <p className="text-2xl font-bold text-primary">{stats.mediaReducaoCBS.toFixed(2)}%</p>
                  <span className="text-xs text-muted-foreground">({stats.totalComReducaoCBS} itens)</span>
                </div>
              </div>
            </div>
          </div>
        )}

        {error && <AlertaErro mensagem={error} fechavel onFechar={() => setError(null)} />}

        <DataTable
          data={classTribs}
          columns={columns}
          getRowKey={(item) => item.id.toString()}
          loading={loading}
          totalItems={total}

          // Paginação manual pois o serviço espera page/pageSize
          // O DataTable suporta paginação interna, mas aqui estamos controlando externamente
          // Para simplificar, vamos deixar o DataTable controlar a UI de paginação se passarmos os props corretos
          // Mas o DataTable atual parece ser client-side ou server-side misto.
          // Vamos usar a prop de paginação do DataTable se ele tiver, ou manter os controles externos se necessário.
          // O DataTable refatorado suporta server-side pagination se passarmos onPageChange?
          // Verificando o código do DataTable anterior: ele usa useDataTable que tem paginação.
          // Se passarmos `data` apenas da página atual, precisamos informar o total.

          // Ações
          rowActions={(item) => (
            <button
              onClick={() => abrirDetalhes(item)}
              className="p-2 text-muted-foreground hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
              title="Visualizar detalhes"
            >
              <Eye className="h-4 w-4" />
            </button>
          )}
        />

        {/* Controles de Paginação Personalizados (se o DataTable não expor controle direto de página externa facilmente) */}
        {!loading && classTribs.length > 0 && (
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-border">
            <div className="text-sm text-muted-foreground">
              Página <span className="font-medium text-primary">{page}</span> de <span className="font-medium text-primary">{Math.ceil(total / pageSize)}</span>
            </div>
            <div className="flex items-center gap-2">
              <button
                onClick={() => setPage(Math.max(1, page - 1))}
                disabled={page === 1}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Anterior
              </button>
              <button
                onClick={() => setPage(page + 1)}
                disabled={page >= Math.ceil(total / pageSize)}
                className="px-3 py-1.5 text-sm font-medium border border-border rounded-lg hover:bg-surface-hover disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Próxima
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Modal de Detalhes */}
      {showModal && classTribSelecionado && (
        <div className="fixed inset-0 z-[100] overflow-y-auto">
          <div
            className="fixed inset-0 bg-black/60 backdrop-blur-sm transition-opacity animate-fade-in"
            onClick={fecharModal}
          />

          <div className="flex min-h-full items-center justify-center p-4">
            <div className="relative w-full max-w-2xl bg-surface rounded-2xl shadow-2xl transform transition-all animate-scale-in border border-border">
              {/* Header */}
              <div className="flex items-center justify-between px-6 py-4 border-b border-border bg-surface-muted/50 rounded-t-2xl">
                <div className="flex items-center gap-3">
                  <div className="p-2 bg-primary/10 rounded-lg">
                    <FileText className="h-5 w-5 text-primary" />
                  </div>
                  <div>
                    <h2 className="text-lg font-bold text-primary">Detalhes da ClassTrib</h2>
                    <p className="text-xs text-muted-foreground font-mono">#{classTribSelecionado.codigoClassTrib}</p>
                  </div>
                </div>
                <button
                  onClick={fecharModal}
                  className="p-2 hover:bg-surface-hover rounded-lg transition-colors text-muted-foreground hover:text-primary"
                >
                  <X className="h-5 w-5" />
                </button>
              </div>

              {/* Content */}
              <div className="p-6 space-y-6 max-h-[70vh] overflow-y-auto custom-scrollbar">
                {/* Informações Principais */}
                <div className="bg-surface-muted/30 rounded-xl p-5 border border-border">
                  <div className="flex items-center gap-2 mb-4">
                    <Tag className="w-4 h-4 text-primary" />
                    <h3 className="text-xs font-bold text-primary uppercase tracking-wider">Informações Principais</h3>
                  </div>

                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                    <div>
                      <p className="text-xs text-muted-foreground mb-1">Código ClassTrib</p>
                      <p className="font-mono font-bold text-lg text-foreground">{classTribSelecionado.codigoClassTrib}</p>
                    </div>
                    <div>
                      <p className="text-xs text-muted-foreground mb-1">CST</p>
                      <span className="inline-block px-3 py-1 bg-blue-50 text-blue-700 text-sm font-bold rounded-lg border border-blue-100">
                        {classTribSelecionado.codigoSituacaoTributaria}
                      </span>
                    </div>
                  </div>

                  <div className="mt-4">
                    <p className="text-xs text-muted-foreground mb-1">Descrição</p>
                    <p className="text-sm text-foreground font-medium">{classTribSelecionado.descricaoClassTrib}</p>
                  </div>
                </div>

                {/* Reduções */}
                <div className="bg-emerald-50/50 rounded-xl p-5 border border-emerald-100">
                  <div className="flex items-center gap-2 mb-4">
                    <Percent className="w-4 h-4 text-emerald-600" />
                    <h3 className="text-xs font-bold text-emerald-700 uppercase tracking-wider">Percentuais de Redução</h3>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="bg-white p-4 rounded-lg border border-emerald-100 shadow-sm">
                      <p className="text-xs text-emerald-600 mb-1 font-bold">Redução IBS</p>
                      <p className="font-mono font-bold text-2xl text-emerald-700">
                        {classTribSelecionado.percentualReducaoIBS.toFixed(2)}%
                      </p>
                    </div>
                    <div className="bg-white p-4 rounded-lg border border-emerald-100 shadow-sm">
                      <p className="text-xs text-emerald-600 mb-1 font-bold">Redução CBS</p>
                      <p className="font-mono font-bold text-2xl text-emerald-700">
                        {classTribSelecionado.percentualReducaoCBS.toFixed(2)}%
                      </p>
                    </div>
                  </div>
                </div>

                {/* Status Grid */}
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                  <div className={`flex items-center gap-3 p-3 rounded-xl border ${classTribSelecionado.validoParaNFe ? 'bg-emerald-50 border-emerald-100' : 'bg-surface-muted border-border'}`}>
                    {classTribSelecionado.validoParaNFe ? (
                      <CheckCircle className="h-5 w-5 text-emerald-600" />
                    ) : (
                      <AlertCircle className="h-5 w-5 text-muted-foreground" />
                    )}
                    <span className={`text-sm font-bold ${classTribSelecionado.validoParaNFe ? 'text-emerald-700' : 'text-muted-foreground'}`}>
                      Válido para NFe
                    </span>
                  </div>

                  <div className={`flex items-center gap-3 p-3 rounded-xl border ${classTribSelecionado.tributacaoRegular ? 'bg-blue-50 border-blue-100' : 'bg-surface-muted border-border'}`}>
                    {classTribSelecionado.tributacaoRegular ? (
                      <CheckCircle className="h-5 w-5 text-blue-600" />
                    ) : (
                      <AlertCircle className="h-5 w-5 text-muted-foreground" />
                    )}
                    <span className={`text-sm font-bold ${classTribSelecionado.tributacaoRegular ? 'text-blue-700' : 'text-muted-foreground'}`}>
                      Tributação Regular
                    </span>
                  </div>
                </div>
              </div>

              {/* Footer */}
              <div className="px-6 py-4 border-t border-border bg-surface-muted/30 rounded-b-2xl flex justify-end gap-3">
                <button
                  onClick={() => handlePrintSingle(classTribSelecionado)}
                  className="px-6 py-2 bg-surface text-primary border border-border rounded-xl hover:bg-surface-hover transition-all font-medium flex items-center gap-2"
                >
                  <Printer className="h-4 w-4" />
                  Imprimir
                </button>
                <button
                  onClick={fecharModal}
                  className="px-6 py-2 bg-primary text-primary-foreground rounded-xl hover:bg-primary/90 transition-all font-medium shadow-lg shadow-primary/20"
                >
                  Fechar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
