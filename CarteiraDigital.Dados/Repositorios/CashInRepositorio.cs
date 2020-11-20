using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Models;
using System.Linq;

namespace CarteiraDigital.Dados.Repositorios
{
    public class CashInRepositorio : Repositorio<CashIn>, ICashInRepositorio
    {
        private readonly IOperacaoExpressao operacaoExpressao;

        public CashInRepositorio(CarteiraDigitalContext context, IOperacaoExpressao operacaoExpressao) : base(context)
        {
            this.operacaoExpressao = operacaoExpressao;
        }

        public bool ExisteCashInEfetivado(int contaId)
        {
            return context.CashIns.Any(operacaoExpressao.DaContaEfetivada<CashIn>(contaId));
        }
    }
}
