using MyMVC.Models;
using Rotativa;
using System.Web.Mvc;

namespace MyMVC.Controllers
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class RelatPosicaoEstoqueController : Controller
    {
        public ActionResult Index()
        {
            var estoque = ProdutoModel.RecuperarRelatPosicaoEstoque();
            return new ViewAsPdf("~/Views/Relatorio/RelatPosicaoEstoqueView.cshtml", estoque);
        }
    }
}