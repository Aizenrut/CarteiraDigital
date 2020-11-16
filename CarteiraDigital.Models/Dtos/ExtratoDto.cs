using CarteiraDigital.Models.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace CarteiraDigital.Models
{
    public struct ExtratoDto
    {
        public string SaldoAtual { get; set; }
        public string SaldoFinalPeriodo { get; set; }
        public string SaldoInicioPeriodo { get; set; }
        public ICollection<ExtratoOperacaoDto> Operacoes { get; set; }

        public static explicit operator ExtratoDto(MovimentacaoDto movimentacaoDto)
        {
            return new ExtratoDto
            {
                SaldoAtual = movimentacaoDto.SaldoAtual.ParaMoeda(),
                SaldoFinalPeriodo = movimentacaoDto.SaldoFinalPeriodo.ParaMoeda(),
                SaldoInicioPeriodo = movimentacaoDto.SaldoInicioPeriodo.ParaMoeda(),
                Operacoes = movimentacaoDto.Operacoes.Select(operacao => (ExtratoOperacaoDto)operacao).ToList()
            };
        }
    }
}
