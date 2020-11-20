using CarteiraDigital.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CarteiraDigital.Dados.Expressoes
{
    public class OperacaoExpressao : IOperacaoExpressao
    {
        public Expression<Func<T, bool>> DaConta<T>(int contaId) where T : Operacao
        {
            return x => x.ContaId == contaId;
        }

        public Expression<Func<T, bool>> Efetivada<T>() where T : Operacao
        {
            return x => x.Status == StatusOperacao.Efetivada;
        }

        public Func<OperacaoDto, bool> Efetivada()
        {
            return x => x.Status == StatusOperacao.Efetivada;
        }

        public Expression<Func<T, bool>> NoPeriodo<T>(DateTime dataInicial, DateTime dataFinal) where T : Operacao
        {
            return x => dataInicial <= x.Data && dataFinal >= x.Data;
        }

        public Expression<Func<T, bool>> DaContaNoPeriodo<T>(int contaId, DateTime dataInicial, DateTime dataFinal) where T : Operacao
        {
            return And<T>(DaConta<T>(contaId), NoPeriodo<T>(dataInicial, dataFinal));
        }

        public Expression<Func<T, bool>> DaContaEfetivada<T>(int contaId) where T : Operacao
        {
            return And<T>(DaConta<T>(contaId), Efetivada<T>());
        }

        private Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> esquerda, Expression<Func<T, bool>> direita) where T : Operacao
        {
            var expressaoInvocada = Expression.Invoke(direita, esquerda.Parameters.Cast<Expression>());
            return (Expression.Lambda<Func<T, bool>>(Expression.AndAlso(esquerda.Body, expressaoInvocada), esquerda.Parameters));
        }
    }
}
