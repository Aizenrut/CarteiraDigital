using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Api.Models
{
    public struct DadosOperacaoBinaria
    {
        [Required]
        public string UsuarioDestino { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        public string Descricao { get; set; }
    }
}
