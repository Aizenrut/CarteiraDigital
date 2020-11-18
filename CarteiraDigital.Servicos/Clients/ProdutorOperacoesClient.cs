using CarteiraDigital.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos.Clients
{
    public class ProdutorOperacoesClient : IProdutorOperacoesClient
    {
        private readonly HttpClient client;

        public ProdutorOperacoesClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task EnfileirarCashIn(EfetivarOperacaoUnariaDto cashIn)
        {
            await client.PostAsync("ProdutorCashIns", ObterConteudo(cashIn));
        }

        public async Task EnfileirarCashOut(EfetivarOperacaoUnariaDto cashOut)
        {
            await client.PostAsync("ProdutorCashOuts", ObterConteudo(cashOut));
        }

        public async Task EnfileirarTransferencia(EfetivarOperacaoBinariaDto transferencia)
        {
            await client.PostAsync("ProdutorTransferencias", ObterConteudo(transferencia));
        }

        public StringContent ObterConteudo(object operacao)
        {
            return new StringContent(content: JsonConvert.SerializeObject(operacao),
                                     encoding: Encoding.UTF8,
                                     mediaType: "application/json");
        }
    }
}
