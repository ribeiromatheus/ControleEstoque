using MyMVC.Models;
using System.Web.Mvc;

namespace MyMVC.Controllers
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class OperEntradaProdutoController : OperEntradaSaidaProdutoController
    {
        protected override string SalvarPedido(EntradaSaidaProdutoViewModel dados)
        {
           return ProdutoModel.SalvarPedidoEntrada(dados.Data, dados.Produtos);
        }
    }
}