namespace SistemaEmpresas.Enums;

/// <summary>
/// Status possíveis de um orçamento
/// </summary>
public enum StatusOrcamento
{
    Aberto,
    EmAnalise,
    Aprovado,
    Rejeitado,
    Cancelado
}

/// <summary>
/// Status possíveis de um pedido
/// </summary>
public enum StatusPedido
{
    Aberto,
    EmProducao,
    Finalizado,
    Cancelado,
    Faturado
}

/// <summary>
/// Status possíveis de uma ordem de serviço
/// </summary>
public enum StatusOrdemServico
{
    Aberta,
    EmAndamento,
    Concluida,
    Cancelada
}

/// <summary>
/// Prioridade de alertas operacionais
/// </summary>
public enum PrioridadeAlerta
{
    Baixa,
    Media,
    Alta,
    Critica
}

/// <summary>
/// Tipo de alerta operacional
/// </summary>
public enum TipoAlerta
{
    EstoqueBaixo,
    PedidoAtrasado,
    OrdemServicoAtrasada,
    VencimentoProximo
}
