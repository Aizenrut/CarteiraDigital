using System.ComponentModel.DataAnnotations.Schema;

namespace CarteiraDigital.Models
{
    public abstract class Entidade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
