using System.ComponentModel.DataAnnotations;

namespace MyMVC.Models
{
    public class LocalArmazenamentoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }
    }
}