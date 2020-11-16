using CarteiraDigital.Models.Enumeracoes;
using System;

namespace CarteiraDigital.Models
{
    public struct OperacaoDto
    {
        public DateTime Data { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public StatusOperacao Status { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public decimal SaldoAnterior { get; set; }
        public string Erro { get; set; }
    }
}
