using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ITransferenciaServico : IOperacaoStrategy<Transferencia, EfetivarOperacaoBinariaDto, OperacaoBinariaDto>
    {
        void ValidarContaDestino(int contaOrigemId, int contaDestinoId);
        int GerarPeloTipo(int contaId, decimal valor, string descricao, TipoMovimentacao tipoTransferencia);
    }
}
