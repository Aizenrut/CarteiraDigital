using CarteiraDigital.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos.Clients
{
    public interface IProdutorOperacoesClient
    {
        Task EnfileirarCashIn(EfetivarOperacaoUnariaDto cashIn);
        Task EnfileirarCashOut(EfetivarOperacaoUnariaDto cashOut);
        Task EnfileirarTransferencia(EfetivarOperacaoBinariaDto transferencia);
        StringContent ObterConteudo(object operacao);
    }
}
