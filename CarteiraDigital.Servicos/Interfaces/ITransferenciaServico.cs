using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ITransferenciaServico
    {
        void Efetivar(OperacaoBinariaDto dto);
        void TransferirPeloTipo(int contaId, decimal valor, string descricao, TipoTransferencia tipoTransferencia);
    }
}
