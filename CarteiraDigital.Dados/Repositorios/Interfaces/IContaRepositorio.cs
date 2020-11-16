using CarteiraDigital.Models;
using System;
using System.Collections.Generic;

namespace CarteiraDigital.Dados.Repositorios
{
    public interface IContaRepositorio : IRepositorio<Conta>
    {
        int ObterIdPeloTitular(string usuarioTitular);
        decimal ObterSaldoAtual(int contaId);
        bool PossuiOperacoes(ICollection<OperacaoDto> operacoes);
        decimal ObterSaldoFinal(ICollection<OperacaoDto> operacoes);
        decimal ObterSaldoInicial(ICollection<OperacaoDto> operacoes);
        MovimentacaoDto ObterMovimentacao(int contaId, DateTime dataInicial, DateTime dataFinal);
        decimal ObterSaldoTemplate(ICollection<OperacaoDto> operacoes, Func<ICollection<OperacaoDto>, decimal> acaoPrincipal);
    }
}
