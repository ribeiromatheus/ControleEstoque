﻿using AutoMapper;
using MyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyMVC.Controllers
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class CadFornecedorController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = Mapper.Map<List<FornecedorViewModel>>(FornecedorModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina));
            var quant = FornecedorModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            ViewBag.Paises = Mapper.Map<List<PaisViewModel>>(PaisModel.RecuperarLista());
            //ViewBag.Cidades = CidadeModel.RecuperarLista();
            //ViewBag.Estados = EstadoModel.RecuperarLista();
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FornecedorPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<FornecedorViewModel>>(FornecedorModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult RecuperarCidadeDoEstado(int idEstado)
        //{
        //    var lista = FornecedorModel.RecuperarLista(idEstado: idEstado);

        //    return Json(lista);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult RecuperarEstadoDoPais(int idPais)
        //{
        //    var lista = EstadoModel.RecuperarLista(idPais: idPais);

        //    return Json(lista);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarFornecedor(int id)
        {
            var vm = Mapper.Map<FornecedorViewModel>(FornecedorModel.RecuperarPeloId(id));
            return Json(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Administrativo")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirFornecedor(int id)
        {
            return Json(FornecedorModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarFornecedor(FornecedorViewModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var vm = Mapper.Map<FornecedorModel>(model);
                    var id = vm.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}