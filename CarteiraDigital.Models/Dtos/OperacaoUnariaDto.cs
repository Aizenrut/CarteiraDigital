using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Models
{
    public struct OperacaoUnariaDto
    {
        [Required]
        public int ContaId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }
        public string Descricao { get; set; }

        public OperacaoUnariaDto(int contaId, decimal valor, string descricao)
        {
            ContaId = contaId;
            Valor = valor;
            Descricao = descricao;
        }
    }
}
