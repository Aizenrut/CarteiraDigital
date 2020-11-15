using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ITransferenciaServico : IOperacaoStrategy<OperacaoBinariaDto>
    {
        void TransferirPeloTipo(int contaId, decimal valor, string descricao, TipoMovimentacao tipoTransferencia);
    }
}
