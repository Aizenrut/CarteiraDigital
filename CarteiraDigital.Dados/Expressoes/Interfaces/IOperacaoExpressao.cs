using CarteiraDigital.Models;
using System;
using System.Linq.Expressions;

namespace CarteiraDigital.Dados.Expressoes
{
    public interface IOperacaoExpressao
    {
        Func<OperacaoDto, bool> Efetivada();
        Expression<Func<T, bool>> NoPeriodo<T>(DateTime dataInicial, DateTime dataFinal) where T : Operacao;
        Expression<Func<T, bool>> DaContaNoPeriodo<T>(int contaId, DateTime dataInicial, DateTime dataFinal) where T : Operacao;
    }
}
