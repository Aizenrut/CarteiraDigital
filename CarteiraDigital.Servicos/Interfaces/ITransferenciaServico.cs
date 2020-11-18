using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ITransferenciaServico : IOperacaoStrategy<Transferencia, EfetivarOperacaoBinariaDto, OperacaoBinariaDto>
    {
        int GerarPeloTipo(int contaId, decimal valor, string descricao, TipoMovimentacao tipoTransferencia);
    }
}
