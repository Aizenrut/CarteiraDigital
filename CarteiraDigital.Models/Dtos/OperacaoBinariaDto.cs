using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Models
{
    public struct OperacaoBinariaDto
    {
        [Required]
        public int ContaOrigemId { get; set; }

        [Required]
        public int ContaDestinoId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        public string Descricao { get; set; }

        public OperacaoBinariaDto(int contaOrigemId, int contaDestinoId, decimal valor, string descricao)
        {
            ContaOrigemId = contaOrigemId;
            ContaDestinoId = contaDestinoId;
            Valor = valor;
            Descricao = descricao;
        }
    }
}
