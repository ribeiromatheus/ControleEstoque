﻿using MyMVC.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyMVC.Controllers.Operacao
{
    [Authorize(Roles = "Gerente,Administrativo")]
    public class OperLacamentoPerdaProdutoController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Inventarios = ProdutoModel.RecuperarListaInventarioComDiferenca();

            return View();
        }

        [HttpGet]
        public JsonResult RecuperarListaProdutoComDiferencaEmInventario(string inventario)
        {
            var ret = ProdutoModel.RecuperarListaProdutoComDiferencaEmInventario(inventario);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Salvar(List<LancamentoPerdaViewModel> dados)
        {
            var ret = ProdutoModel.SalvarLancamentoPerda(dados);
            return Json(ret);
        }
    }
}