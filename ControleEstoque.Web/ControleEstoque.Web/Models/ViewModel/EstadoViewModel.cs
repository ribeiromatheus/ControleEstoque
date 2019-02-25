using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Web.Models
{
    public class EstadoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha a UF.")]
        [MaxLength(2, ErrorMessage = "A UF deve ter 2 caracteres.")]
        public string UF { get; set; }

        public bool Ativo { get; set; }

        [Required(ErrorMessage = "Selecione o país.")]
        public int IdPais { get; set; }
    }
}