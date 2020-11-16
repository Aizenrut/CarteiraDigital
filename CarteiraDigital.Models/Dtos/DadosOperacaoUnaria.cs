using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Models
{
    public struct DadosOperacaoUnaria
    {
        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        public string Descricao { get; set; }
    }
}
