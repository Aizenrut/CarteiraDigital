using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashInServico
    {
        bool EhPrimeiroCashIn(int contaId);
        void Efetivar(OperacaoUnariaDto dto);
        CashIn GerarCashIn(Conta conta, decimal valor, string descricao);
    }
}
