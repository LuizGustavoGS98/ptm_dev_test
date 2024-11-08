using System.ComponentModel.DataAnnotations;

namespace ptm_dev_test.Models
{
    public class ExamesModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Idade é obrigatório.")]
        [Range(1, 120, ErrorMessage = "A idade deve estar entre 1 e 120 anos.")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O campo Gênero é obrigatório.")]
        public string Genero { get; set; }
    }
}
