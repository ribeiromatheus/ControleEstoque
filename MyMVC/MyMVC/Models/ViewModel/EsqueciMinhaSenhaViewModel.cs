using System.ComponentModel.DataAnnotations;

namespace MyMVC.Models
{
    public class EsqueciMinhaSenhaViewModel
    {
        [Required(ErrorMessage = "Infome o login.")]
        public string Login { get; set; }
    }
}