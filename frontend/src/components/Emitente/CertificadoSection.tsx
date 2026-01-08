import { useState } from 'react';
import {
  Shield,
  Upload,
  CheckCircle,
  AlertTriangle,
  XCircle,
  File,
  Loader2,
  Trash2,
} from 'lucide-react';
import { emitenteService } from '../../services/Emitentes/emitenteService';
import { ModalConfirmacao } from '../common';

interface CertificadoInfo {
  subject: string;
  issuer: string;
  validoApartirDe: string;
  validoAte: string;
  estaValido: boolean;
  estaProximoDoVencimento: boolean;
  diasRestantes: number;
  thumbprint?: string;
  serialNumber?: string;
  possuiChavePrivada?: boolean;
}

interface CertificadoSectionProps {
  emitenteId: number;
  possuiCertificado: boolean;
  certificadoValido: boolean;
  validadeCertificado?: Date | string | null;
  onCertificadoAtualizado?: () => void;
}

export default function CertificadoSection({
  emitenteId,
  possuiCertificado,
  certificadoValido,
  validadeCertificado,
  onCertificadoAtualizado,
}: CertificadoSectionProps) {
  const [arquivo, setArquivo] = useState<File | null>(null);
  const [senha, setSenha] = useState('');
  const [uploading, setUploading] = useState(false);
  const [validating, setValidating] = useState(false);
  const [removing, setRemoving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [certificadoInfo, setCertificadoInfo] = useState<CertificadoInfo | null>(null);
  const [mostrarInfo, setMostrarInfo] = useState(false);
  const [modalRemoverAberto, setModalRemoverAberto] = useState(false);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      // Validar extensão
      const extensao = file.name.toLowerCase();
      if (!extensao.endsWith('.pfx') && !extensao.endsWith('.p12')) {
        setError('Apenas arquivos .pfx ou .p12 são aceitos');
        setArquivo(null);
        return;
      }
      setArquivo(file);
      setError(null);
    }
  };

  const handleUpload = async () => {
    if (!arquivo || !senha) {
      setError('Selecione um arquivo e informe a senha');
      return;
    }

    try {
      setUploading(true);
      setError(null);
      setSuccess(null);

      const resultado = await emitenteService.uploadCertificado(emitenteId, arquivo, senha);

      setSuccess('Certificado carregado com sucesso!');
      setCertificadoInfo(resultado.certificado);
      setArquivo(null);
      setSenha('');
      setMostrarInfo(true);

      // Notificar componente pai
      if (onCertificadoAtualizado) {
        onCertificadoAtualizado();
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar certificado');
    } finally {
      setUploading(false);
    }
  };

  const handleValidar = async () => {
    try {
      setValidating(true);
      setError(null);
      setCertificadoInfo(null);

      const resultado = await emitenteService.validarCertificado(emitenteId);

      setCertificadoInfo(resultado.certificado);
      setMostrarInfo(true);

      if (!resultado.valido) {
        setError(resultado.mensagem);
      } else {
        setSuccess(resultado.mensagem);
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao validar certificado');
    } finally {
      setValidating(false);
    }
  };

  const handleRemover = () => {
    setModalRemoverAberto(true);
  };

  const confirmarRemover = async () => {
    try {
      setRemoving(true);
      setError(null);
      setModalRemoverAberto(false);

      await emitenteService.removerCertificado(emitenteId);

      setSuccess('Certificado removido com sucesso!');
      setCertificadoInfo(null);
      setMostrarInfo(false);

      // Notificar componente pai
      if (onCertificadoAtualizado) {
        onCertificadoAtualizado();
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao remover certificado');
    } finally {
      setRemoving(false);
    }
  };

  const formatarData = (data: string) => {
    return new Date(data).toLocaleDateString('pt-BR');
  };

  const getStatusIcon = () => {
    if (!possuiCertificado) {
      return <XCircle className="w-5 h-5 text-gray-400" />;
    }
    if (certificadoValido) {
      return <CheckCircle className="w-5 h-5 text-green-600" />;
    }
    return <AlertTriangle className="w-5 h-5 text-yellow-600" />;
  };

  const getStatusText = () => {
    if (!possuiCertificado) {
      return 'Nenhum certificado configurado';
    }
    if (certificadoValido) {
      return `Certificado válido até ${validadeCertificado ? formatarData(validadeCertificado.toString()) : 'N/A'}`;
    }
    return 'Certificado expirado ou inválido';
  };

  const getStatusColor = () => {
    if (!possuiCertificado) return 'text-gray-600';
    if (certificadoValido) return 'text-green-600';
    return 'text-yellow-600';
  };

  return (
    <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
      <div className="flex items-center justify-between mb-6">
        <div className="flex items-center gap-3">
          <Shield className="w-6 h-6 text-indigo-600" />
          <div>
            <h2 className="text-lg font-semibold text-gray-900">
              Certificado Digital
            </h2>
            <p className="text-sm text-gray-600 flex items-center gap-2 mt-1">
              {getStatusIcon()}
              <span className={getStatusColor()}>{getStatusText()}</span>
            </p>
          </div>
        </div>

        {possuiCertificado && (
          <div className="flex gap-2">
            <button
              type="button"
              onClick={handleValidar}
              disabled={validating}
              className="btn-secondary text-sm"
            >
              {validating ? (
                <>
                  <Loader2 className="w-4 h-4 animate-spin" />
                  Validando...
                </>
              ) : (
                <>
                  <CheckCircle className="w-4 h-4" />
                  Validar
                </>
              )}
            </button>
            <button
              type="button"
              onClick={handleRemover}
              disabled={removing}
              className="btn-danger text-sm"
            >
              {removing ? (
                <>
                  <Loader2 className="w-4 h-4 animate-spin" />
                  Removendo...
                </>
              ) : (
                <>
                  <Trash2 className="w-4 h-4" />
                  Remover
                </>
              )}
            </button>
          </div>
        )}
      </div>

      {/* Mensagens */}
      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-md flex items-start gap-2">
          <XCircle className="w-5 h-5 text-red-600 flex-shrink-0 mt-0.5" />
          <span className="text-sm text-red-700">{error}</span>
        </div>
      )}

      {success && (
        <div className="mb-4 p-3 bg-green-50 border border-green-200 rounded-md flex items-start gap-2">
          <CheckCircle className="w-5 h-5 text-green-600 flex-shrink-0 mt-0.5" />
          <span className="text-sm text-green-700">{success}</span>
        </div>
      )}

      {/* Informações do Certificado */}
      {mostrarInfo && certificadoInfo && (
        <div className="mb-6 p-4 bg-gray-50 rounded-lg border border-gray-200">
          <h3 className="font-medium text-gray-900 mb-3 flex items-center gap-2">
            <File className="w-4 h-4" />
            Informações do Certificado
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3 text-sm">
            <div>
              <span className="text-gray-600">Titular:</span>
              <p className="font-medium text-gray-900 mt-1">{certificadoInfo.subject}</p>
            </div>
            <div>
              <span className="text-gray-600">Emissor:</span>
              <p className="font-medium text-gray-900 mt-1">{certificadoInfo.issuer}</p>
            </div>
            <div>
              <span className="text-gray-600">Válido de:</span>
              <p className="font-medium text-gray-900 mt-1">
                {formatarData(certificadoInfo.validoApartirDe)}
              </p>
            </div>
            <div>
              <span className="text-gray-600">Válido até:</span>
              <p className="font-medium text-gray-900 mt-1 flex items-center gap-2">
                {formatarData(certificadoInfo.validoAte)}
                {certificadoInfo.estaProximoDoVencimento && (
                  <span className="text-yellow-600 text-xs">
                    ({certificadoInfo.diasRestantes} dias restantes)
                  </span>
                )}
              </p>
            </div>
            <div className="md:col-span-2">
              <span className="text-gray-600">Status:</span>
              <p className="font-medium mt-1 flex items-center gap-2">
                {certificadoInfo.estaValido ? (
                  <>
                    <CheckCircle className="w-4 h-4 text-green-600" />
                    <span className="text-green-600">Válido e ativo</span>
                  </>
                ) : (
                  <>
                    <XCircle className="w-4 h-4 text-red-600" />
                    <span className="text-red-600">Inválido ou expirado</span>
                  </>
                )}
              </p>
            </div>
          </div>
        </div>
      )}

      {/* Upload de Certificado */}
      <div className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Arquivo do Certificado (.pfx ou .p12)
          </label>
          <div className="flex items-center gap-3">
            <label className="btn-secondary cursor-pointer">
              <Upload className="w-4 h-4" />
              Selecionar Arquivo
              <input
                type="file"
                accept=".pfx,.p12"
                onChange={handleFileChange}
                className="hidden"
              />
            </label>
            {arquivo && (
              <span className="text-sm text-gray-600 flex items-center gap-2">
                <File className="w-4 h-4" />
                {arquivo.name}
              </span>
            )}
          </div>
        </div>

        <div>
          <label htmlFor="senhaCertificado" className="block text-sm font-medium text-gray-700 mb-2">
            Senha do Certificado
          </label>
          <input
            type="password"
            id="senhaCertificado"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            placeholder="Digite a senha do certificado"
            className="input-field"
          />
          <p className="text-xs text-gray-500 mt-1">
            A senha será armazenada de forma criptografada no banco de dados
          </p>
        </div>

        <button
          type="button"
          onClick={handleUpload}
          disabled={uploading || !arquivo || !senha}
          className="btn-primary w-full sm:w-auto"
        >
          {uploading ? (
            <>
              <Loader2 className="w-4 h-4 animate-spin" />
              Carregando certificado...
            </>
          ) : (
            <>
              <Shield className="w-4 h-4" />
              Carregar Certificado
            </>
          )}
        </button>
      </div>

      <ModalConfirmacao
        aberto={modalRemoverAberto}
        titulo="Remover Certificado"
        mensagem="Tem certeza que deseja remover o certificado digital? Esta ação impedirá a emissão de notas fiscais até que um novo certificado seja carregado."
        onConfirmar={confirmarRemover}
        onCancelar={() => setModalRemoverAberto(false)}
        processando={removing}
      />
    </div>
  );
}
