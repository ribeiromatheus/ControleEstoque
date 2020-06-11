using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index() => View();

        public ActionResult Sobre() => View();
    }
}