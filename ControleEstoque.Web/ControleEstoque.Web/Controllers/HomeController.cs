using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult Sobre() => View();
    }
}