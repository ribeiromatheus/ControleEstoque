using MyMVC.Models;
using System.Web.Mvc;

namespace MyMVC.Controllers
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class OperSaidaProdutoController : OperEntradaSaidaProdutoController
    {
        protected override string SalvarPedido(EntradaSaidaProdutoViewModel dados)
        {
            return ProdutoModel.SalvarPedidoSaida(dados.Data, dados.Produtos);
        }
    }
}