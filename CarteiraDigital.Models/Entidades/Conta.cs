using System.Collections.Generic;

namespace CarteiraDigital.Models
{
    public class Conta : Entidade
    {
        public string UsuarioTitular { get; set; }
        public decimal Saldo { get; set; }
        public ICollection<CashIn> CashIns { get; set; }
        public ICollection<CashOut> CashOuts { get; set; }
        public ICollection<Transferencia> Transferencias { get; set; }

        public Conta()
        {
            CashIns = new List<CashIn>();
            CashOuts = new List<CashOut>();
            Transferencias = new List<Transferencia>();
        }

        public Conta(string usuarioTitular) : this()
        {
            UsuarioTitular = usuarioTitular;
        }
    }
}
