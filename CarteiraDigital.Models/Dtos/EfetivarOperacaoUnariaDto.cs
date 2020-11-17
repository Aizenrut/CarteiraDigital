namespace CarteiraDigital.Models
{
    public struct EfetivarOperacaoUnariaDto
    {
        public int OperacaoId { get; set; }

        public EfetivarOperacaoUnariaDto(int operacaoId)
        {
            OperacaoId = operacaoId;
        }
    }
}
