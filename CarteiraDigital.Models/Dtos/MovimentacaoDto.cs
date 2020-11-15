using System.Collections.Generic;

namespace CarteiraDigital.Models
{
    public struct MovimentacaoDto
    {
        public decimal SaldoAtual { get; }
        public decimal SaldoFinalPeriodo { get; }
        public decimal SaldoInicioPeriodo { get; }
        public ICollection<OperacaoDto> Operacoes { get; }

        public MovimentacaoDto(decimal saldoAtual, decimal saldoFinalPeriodo, decimal saldoInicioPeriodo, ICollection<OperacaoDto> operacoes)
        {
            SaldoAtual = saldoAtual;
            SaldoFinalPeriodo = saldoFinalPeriodo;
            SaldoInicioPeriodo = saldoInicioPeriodo;
            Operacoes = operacoes;
        }
    }
}
