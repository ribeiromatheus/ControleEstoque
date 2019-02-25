using ControleEstoque.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class HomeController : Controller
    {
        //private const int _tamBloco = 2;

        //private List<ListaProdutoViewModel> _lista = new List<ListaProdutoViewModel>()
        //{
        //    new ListaProdutoViewModel { Nome = "TV", Preco = 1500 },
        //    new ListaProdutoViewModel { Nome = "Notebook", Preco = 1500 },
        //    new ListaProdutoViewModel { Nome = "Mouse", Preco = 1500 },
        //    new ListaProdutoViewModel { Nome = "Teclado", Preco = 1500 },
        //    new ListaProdutoViewModel { Nome = "Monitor", Preco = 1500 },
        //    new ListaProdutoViewModel { Nome = "Pendrive", Preco = 1500 },
        //};

        //private List<ListaPaisViewModel> _listaPais = new List<ListaPaisViewModel>()
        //{
        //    new ListaPaisViewModel { Id = 1, Nome = "Estados Unidos" },
        //    new ListaPaisViewModel { Id = 2, Nome = "Canadá" },
        //    new ListaPaisViewModel { Id = 3, Nome = "Brasil" }
            
        //};

        //private List<ListaEstadoViewModel> _listaEstados = new List<ListaEstadoViewModel>()
        //{
        //    new ListaEstadoViewModel { Id = 1, Nome = "Los Angeles", IdPais = 1 },
        //    new ListaEstadoViewModel { Id = 2, Nome = "São Paulo", IdPais = 3 }

        //};

        //private List<ListaCidadeViewModel> _listaCidade = new List<ListaCidadeViewModel>()
        //{
        //    new ListaCidadeViewModel { Id = 1, Nome = "Califórnia", IdEstado = 1 },
        //    new ListaCidadeViewModel { Id = 2, Nome = "São Paulo", IdEstado = 2 }

        //};

        //private List<ListaProdutoViewModel> ObterBlocoProduto(int bloco)
        //{
        //    var posInicial = _tamBloco * (bloco - 1);
        //    return _lista.Skip(posInicial).Take(_tamBloco).ToList();
        //}

        //private string CarregarBlocoProduto(List<ListaProdutoViewModel> lista)
        //{
        //    var ret = string.Empty;

        //    ViewData.Model = lista;

        //    var viewProduto = ViewEngines.Engines.FindPartialView(ControllerContext, "_BlocoProdutoPartial");
        //    using (var writer = new StringWriter())
        //    {
        //        var viewContext = new ViewContext(ControllerContext, viewProduto.View, ViewData, TempData, writer);
        //        viewProduto.View.Render(viewContext, writer);
        //        ret = writer.GetStringBuilder().ToString();
        //    }
        //    return ret;
        //}

        //private bool VerificarUltimoBloco(int bloco)
        //{
        //    var posInicial = _tamBloco * (bloco - 1);
        //    var ultimoItem = (posInicial + _tamBloco);
        //    return (ultimoItem >= _lista.Count);
        //}

        public ActionResult Index()
        {
            //return View(this.ObterBlocoProduto(1));

            //var ret = new List<ListaPaisViewModel>();
            //ret.AddRange(_listaPais);
            //ret.Insert(0, new ListaPaisViewModel() { Id = -1, Nome = "--" });

            //return View(ret);
            return View();
        }

        //[HttpPost]
        //public ActionResult ObterEstados(int idPais)
        //{
        //    System.Threading.Thread.Sleep(2000);

        //    var ret = _listaEstados.FindAll(x => x.IdPais == idPais);

        //    if (ret.Count > 0)
        //    {
        //        ret.Insert(0, new ListaEstadoViewModel() { Id = -1, Nome = "--" });
        //    }

        //    return Json(ret);
        //}

        //[HttpPost]
        //public ActionResult ObterCidades(int idEstado)
        //{
        //    System.Threading.Thread.Sleep(2000);

        //    var ret = _listaCidade.FindAll(x => x.IdEstado == idEstado);

        //    if (ret.Count > 0)
        //    {
        //        ret.Insert(0, new ListaCidadeViewModel() { Id = -1, Nome = "--" });
        //    }

        //    return Json(ret);
        //}

        //public ActionResult ListarProduto(List<ListaProdutoViewModel> lista)
        //{
        //    return PartialView("_BlocoProdutoPartial", lista);
        //}

        //[HttpPost]
        //public ActionResult CarregarBlocoProduto(int bloco)
        //{
        //    System.Threading.Thread.Sleep(1000);
        //    var listaProduto = this.ObterBlocoProduto(bloco);
        //    return Json(new BlocoProdutoViewModel()
        //    {
        //        Html = this.CarregarBlocoProduto(listaProduto),
        //        UltimoBloco = this.VerificarUltimoBloco(bloco)
        //    });
        //}

        public ActionResult Sobre()
        {
            return View();
        }
    }
}