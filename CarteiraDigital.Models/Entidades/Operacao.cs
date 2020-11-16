using System;

namespace CarteiraDigital.Models
{
    public abstract class Operacao : Entidade
    {
        public int ContaId { get; set; }
        public Conta Conta { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public StatusOperacao Status { get; set; }
        public decimal SaldoAnterior { get; set; }
        public string Erro { get; set; }

        public Operacao()
        {
        }

        protected Operacao(int contaId, decimal valor, string descricao, decimal saldoAnterior)
        {
            ContaId = contaId;
            Valor = valor;
            Descricao = descricao;
            Data = DateTime.Now;
            SaldoAnterior = saldoAnterior;
        }
    }
}
