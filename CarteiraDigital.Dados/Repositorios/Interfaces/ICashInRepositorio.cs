using CarteiraDigital.Models;

namespace CarteiraDigital.Dados.Repositorios
{
    public interface ICashInRepositorio : IRepositorio<CashIn>
    {
        bool ExisteCashInEfetivado(int contaId);
    }
}
