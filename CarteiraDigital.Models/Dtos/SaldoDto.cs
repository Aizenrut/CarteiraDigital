namespace CarteiraDigital.Models
{
    public struct SaldoDto
    {
        public string Saldo { get; }

        public SaldoDto(string saldo)
        {
            Saldo = saldo;
        }

        public static explicit operator SaldoDto(string saldo)
        {
            return new SaldoDto(saldo);
        }
    }
}
