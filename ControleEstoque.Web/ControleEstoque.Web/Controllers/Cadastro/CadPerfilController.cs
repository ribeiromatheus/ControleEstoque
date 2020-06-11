using AutoMapper;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class CadPerfilController : BaseController
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index()
        {
            ViewBag.ListaUsuario = Mapper.Map<List<UsuarioViewModel>>(UsuarioModel.RecuperarLista());
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = Mapper.Map<List<PerfilViewModel>>(PerfilModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina));
            var quant = PerfilModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PerfilPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<PerfilViewModel>>(PerfilModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarPerfil(int id)
        {
            var ret = Mapper.Map<PerfilViewModel>(PerfilModel.RecuperarPeloId(id));
            //ret.CarregarUsuarios();
            return Json(ret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirPerfil(int id) => Json(PerfilModel.ExcluirPeloId(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarPerfil(PerfilViewModel model, List<int> idUsuarios)
        {
            // Criar variável para armazena o resultado
            var resultado = "OK";
            // Criar variável para armazena as mensagens
            var mensagens = new List<string>();
            // Criar variável para armazena o id salvo
            var idSalvo = string.Empty;

            // Se o ModelState não for válido
            if (!ModelState.IsValid)
            {
                // Popula a variável resultado com "AVISO"
                resultado = "AVISO";
                // Popula a variável mensagens com o erro
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            // Senão
            else
            {
                /* Cria uma nova instância do método Usuarios do PerfilModel com a uma lista
                 * do tipo UsuarioModel
                 */
                model.Usuarios = new List<UsuarioViewModel>();

                // Se idUsuarios for nulo ou a contagem for  0 (zero)
                if (idUsuarios == null || idUsuarios.Count == 0)
                {
                    // Adiciona na lista com o ID -1
                    model.Usuarios.Add(new UsuarioViewModel() { Id = -1 });
                }

                // Senão
                else
                {
                    /* Laço de repetição foreach
                     * Criar variável id para armazenar o que vier no parâmetro
                     * idUsuarios
                     */
                    foreach (var id in idUsuarios)
                    {
                        /* Adiciona na lista, populando o que vier na variável id para o 
                         * método Id { get; set; } 
                         */
                        model.Usuarios.Add(new UsuarioViewModel() { Id = id });
                    }
                }

                try
                {
                    // Criar variável vm para armazenar o mapeamento
                    var vm = Mapper.Map<PerfilModel>(model);
                    // Criar variável id para armazenar o método Salvar()
                    var id = vm.Salvar();

                    // Se o id for maior que zero
                    if (id > 0)
                    {
                        // Popula a variável idSalvo, convertendo o id para string
                        idSalvo = id.ToString();
                    }
                    // Senão
                    else
                    {
                        // Popula a variável resultado com "ERRO"
                        resultado = "ERRO";
                    }
                }
                // Erro de excessão
                catch (Exception ex)
                {
                    // Popula a variável resultado com "ERRO"
                    resultado = "ERRO";
                }
            }
            // Retorna em formato Json o que vier nas variáveis resultado, mensagens e idSalvo
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}