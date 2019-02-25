using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class ProdutoModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int QuantEstoque { get; set; }
        public int IdUnidadeMedida { get; set; }
        public virtual UnidadeMedidaModel UnidadeMedida { get; set; }
        public int IdGrupo { get; set; }
        public virtual GrupoProdutoModel Grupo { get; set; }
        public int IdMarca { get; set; }
        public virtual MarcaProdutoModel Marca { get; set; }
        public int IdFornecedor { get; set; }
        public virtual FornecedorModel Fornecedor { get; set; }
        public int IdLocalArmazenamento { get; set; }
        public virtual LocalArmazenamentoModel LocalArmazenamento { get; set; }
        public bool Ativo { get; set; }
        public string Imagem { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Produtos.Count();
            }
            return ret;
        }

        //private static ProdutoModel MontarProduto(SqlDataReader reader)
        //{
        //    return new ProdutoModel
        //    {
        //        Id = (int)reader["id"],
        //        Codigo = (string)reader["codigo"],
        //        Nome = (string)reader["nome"],
        //        PrecoCusto = (decimal)reader["preco_custo"],
        //        PrecoVenda = (decimal)reader["preco_venda"],
        //        QuantEstoque = (int)reader["quant_estoque"],
        //        IdUnidadeMedida = (int)reader["id_unidade_medida"],
        //        IdGrupo = (int)reader["id_grupo"],
        //        IdMarca = (int)reader["id_marca"],
        //        IdFornecedor = (int)reader["id_fornecedor"],
        //        IdLocalArmazenamento = (int)reader["id_local_armazenamento"],
        //        Ativo = (bool)reader["ativo"],
        //        Imagem = (string)reader["imagem"]
        //    };
        //}

        //private static ProdutoInventarioViewModel MontarProdutoInventario(SqlDataReader reader)
        //{
        //    return new ProdutoInventarioViewModel
        //    {
        //        Id = (int)reader["id"],
        //        Codigo = (string)reader["codigo"],
        //        NomeProduto = (string)reader["nome_produto"],
        //        NomeLocalArmazenamento = (string)reader["nome_local_armazenamento"],
        //        QuantEstoque = (int)reader["quant_estoque"],
        //        NomeUnidadeMedida = (string)reader["nome_unidade_medida"]

        //    };
        //}

        public static List<ProdutoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", bool somenteAtivos = false)
        {
            var ret = new List<ProdutoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" where (lower(nome) like '%{0}%')", filtro.ToLower());
                }

                if (somenteAtivos)
                {
                    filtroWhere = (string.IsNullOrEmpty(filtroWhere) ? " where" : " and") + "(ativo = 1)";
                }

                var paginacao = "";
                var pos = (pagina - 1) * tamPagina;
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = string.Format(" offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                }

                var sql =
                    "select id, codigo, nome, ativo, imagem, preco_custo as PrecoCusto," +
                    " preco_venda as PrecoVenda, quant_estoque as QuantEstoque, id_unidade_medida" +
                    " as IdUnidadeMedida, id_grupo as IdGrupo, id_marca as IdMarca, id_fornecedor" +
                    " as IdFornecedor, id_local_armazenamento as IdLocalArmazenamento from produto " +
                    filtroWhere +
                    " order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                    paginacao;
                ret = db.Database.Connection.Query<ProdutoModel>(sql).ToList();
            }

            return ret;
        }

        public static ProdutoModel RecuperarPeloId(int id)
        {
            ProdutoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Produtos.Find(id);
            }
            return ret;
        }

        public static string RecuperarImagemPeloId(int id)
        {
            string ret = "";

            using (var db = new ContextoBD())
            {
                ret = db.Produtos
                    .Where(x => x.Id == id)
                    .Select(x => x.Imagem)
                    .SingleOrDefault();
            }
            return ret;
        }

        public static List<RelatPosicaoEstoqueViewModel> RecuperarRelatPosicaoEstoque()
        {
            var ret = new List<RelatPosicaoEstoqueViewModel>();

            using (var db = new ContextoBD())
            {
                ret = db.Produtos
                    .Where(x => x.Ativo)
                    .OrderBy(x => x.Nome)
                    .Select(x => new RelatPosicaoEstoqueViewModel()
                    {
                        CodigoProduto = x.Codigo,
                        NomeProduto = x.Nome,
                        QuantidadeProduto = x.QuantEstoque
                    }).ToList();
            }

            return ret;
        }

        public static List<RelatRessuprimentoViewModel> RecuperarRelatRessuprimento(int minimo)
        {
            var ret = new List<RelatRessuprimentoViewModel>();

            using (var db = new ContextoBD())
            {
                ret = db.Produtos
                    .Where(x => x.Ativo && x.QuantEstoque < minimo)
                    .OrderBy(x => x.QuantEstoque)
                    .ThenBy(x => x.Nome)
                    .Select(x => new RelatRessuprimentoViewModel()
                    {
                        CodigoProduto = x.Codigo,
                        NomeProduto = x.Nome,
                        QuantidadeProduto = x.QuantEstoque,
                        Compra = (minimo - x.QuantEstoque)
                    }).ToList();
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var db = new ContextoBD())
                {
                    var produto = new ProdutoModel { Id = id };
                    db.Produtos.Attach(produto);
                    db.Entry(produto).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }
            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var db = new ContextoBD())
            {
                if (model == null)
                {
                    db.Produtos.Add(this);
                }
                else
                {
                    db.Produtos.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                }
                db.SaveChanges();
                ret = this.Id;
            }
            return ret;
        }

        public static string SalvarPedidoEntrada(DateTime data, Dictionary<int, int> produtos)
        {
            return SalvarPedido(data, produtos, "entrada_produto", true);
        }

        public static string SalvarPedidoSaida(DateTime data, Dictionary<int, int> produtos)
        {
            return SalvarPedido(data, produtos, "saida_produto", false);
        }

        private static string SalvarPedido(DateTime data, Dictionary<int, int> produtos, string nomeTabela, bool entrada)
        {
            var ret = "";

            try
            {
                using (var db = new ContextoBD())
                {
                    db.Database.Connection.Open();

                    var numPedido = db.Database.Connection.ExecuteScalar<int>
                        ($"SELECT NEXT VALUE FOR sec_{nomeTabela}").ToString("D10");

                    //using (var comando = new SqlCommand())
                    //{
                    //    comando.Connection = conexao;
                    //    comando.CommandText = $"SELECT NEXT VALUE FOR sec_{nomeTabela}";
                    //    numPedido = ((int)comando.ExecuteScalar()).ToString("D10");
                    //}

                    using (var transacao = db.Database.Connection.BeginTransaction())
                    {
                        foreach (var produto in produtos)
                        {
                            var sql = $"INSERT INTO {nomeTabela} (numero, data, id_produto, quant) VALUES (@numero, @data, @id_produto, @quant)";
                            var parametrosInsert = new { numero = numPedido, data, id_produto = produto.Key, quant = produto.Value };
                            db.Database.Connection.Execute(sql, parametrosInsert, transacao);

                            //using (var comando = new SqlCommand())
                            //{
                            //    comando.Connection = conexao;
                            //    comando.Transaction = transacao;
                            //    comando.CommandText = $"INSERT INTO {nomeTabela} (numero, data, id_produto, quant) VALUES (@numero, @data, @id_produto, @quant)";

                            //    comando.Parameters.Add("@numero", SqlDbType.VarChar).Value = numPedido;
                            //    comando.Parameters.Add("@data", SqlDbType.Date).Value = data;
                            //    comando.Parameters.Add("@id_produto", SqlDbType.Int).Value = produto.Key;
                            //    comando.Parameters.Add("@quant", SqlDbType.Int).Value = produto.Value;

                            //    comando.ExecuteNonQuery();
                            //}

                            var sinal = (entrada ? "+" : "-");
                            sql = $"UPDATE produto SET quant_estoque = quant_estoque {sinal} @quant_estoque WHERE (id = @id)";
                            var parametrosUpdate = new { id = produto.Key, quant_estoque = produto.Value };
                            db.Database.Connection.Execute(sql, parametrosUpdate, transacao);

                            //using (var comando = new SqlCommand())
                            //{
                            //    var sinal = (entrada ? "+" : "-");
                            //    comando.Connection = conexao;
                            //    comando.Transaction = transacao;
                            //    comando.CommandText = $"UPDATE produto SET quant_estoque = quant_estoque {sinal} @quant_estoque WHERE (id = @id)";

                            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = produto.Key;
                            //    comando.Parameters.Add("@quant_estoque", SqlDbType.Int).Value = produto.Value;

                            //    comando.ExecuteNonQuery();
                            //}

                        }
                        transacao.Commit();

                        ret = numPedido;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return ret;
        }

        public static List<ProdutoInventarioViewModel> RecuperarListaParaInventario()
        {
            var ret = new List<ProdutoInventarioViewModel>();

            using (var db = new ContextoBD())
            {
                var sql = "select p.id, p.codigo, p.nome as NomeProduto, " +
                    "p.quant_estoque as QuantEstoque, l.nome as NomeLocalArmazenamento, " +
                    "u.nome as NomeUnidadeMedida " +
                    "from produto p, tb_locaisArmazenamentos l, tb_unidadeMedida u " +
                    "where (p.ativo = 1) and " +
                    "(p.id_local_armazenamento = l.id) and " +
                    "(p.id_unidade_medida = u.id) " +
                    "order by l.nome, p.nome";
                ret = db.Database.Connection.Query<ProdutoInventarioViewModel>(sql).ToList();

            }
            return ret;
        }

        public static bool SalvarInventario(List<ItemInventarioViewModel> dados)
        {
            var ret = true;

            try
            {
                var data = DateTime.Now;

                using (var db = new ContextoBD())
                {
                    foreach (var produtoInventario in dados)
                    {
                        db.InventariosEstoques.Add(new InventarioEstoqueModel
                        {
                            Data = data,
                            IdProduto = produtoInventario.IdProduto,
                            QuantidadeEstoque = produtoInventario.QuantidadeEstoque,
                            QuantidadeInventario = produtoInventario.QuantidadeInventario,
                            Motivo = produtoInventario.Motivo
                        });
                    }
                    db.SaveChanges();
                    //using (var transacao = db.Database.Connection.BeginTransaction())
                    //{
                    //    foreach (var produtoInventario in dados)
                    //    {
                    //        var sql = "insert into inventario_estoque (data, id_produto, " +
                    //            "quant_estoque, quant_inventario, motivo) values (@data, " +
                    //            "@id_produto, @quant_estoque, @quant_inventario, @motivo)";
                    //        var parametros = new
                    //        {
                    //            data,
                    //            id_produto = produtoInventario.IdProduto,
                    //            quant_estoque = produtoInventario.QuantidadeEstoque,
                    //            quant_inventario = produtoInventario.QuantidadeInventario,
                    //            motivo = produtoInventario.Motivo
                    //        };
                    //        //comando.Parameters.Add("@data", SqlDbType.DateTime).Value = data;
                    //        //comando.Parameters.Add("@id_produto", SqlDbType.Int).Value = produtoInventario.IdProduto;
                    //        //comando.Parameters.Add("@quant_estoque", SqlDbType.Int).Value = produtoInventario.QuantidadeEstoque;
                    //        //comando.Parameters.Add("@quant_inventario", SqlDbType.Int).Value = produtoInventario.QuantidadeInventario;
                    //        //comando.Parameters.Add("@motivo", SqlDbType.VarChar).Value = produtoInventario.Motivo ?? "";
                    //        ret = (db.Database.Connection.Execute(sql, parametros, transacao) > 0);
                    //        //comando.ExecuteNonQuery();
                    //    }
                    //    transacao.Commit();
                    //}
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }

        public static List<InventarioComDiferencaViewModel> RecuperarListaInventarioComDiferenca()
        {
            var ret = new List<InventarioComDiferencaViewModel>();

            using (var db = new ContextoBD())
            {
                var dados = db.InventariosEstoques
                    .Where(x => x.QuantidadeEstoque > x.QuantidadeInventario)
                    .OrderBy(x => x.Data)
                    // Order by data desconsiderando a hora
                    .GroupBy(x => new
                    {
                        Ano = x.Data.Year,
                        Mes = x.Data.Month,
                        Dia = x.Data.Day,
                        x.Produto.IdLocalArmazenamento,
                        NomeLocalArmazenamento = x.Produto.LocalArmazenamento.Nome
                    })
                    .Select(g => new
                    {
                        g.Key.Ano,
                        g.Key.Mes,
                        g.Key.Dia,
                        g.Key.IdLocalArmazenamento,
                        g.Key.NomeLocalArmazenamento
                    })
                    .ToList();

                foreach (var item in dados)
                {
                    var data = new DateTime(item.Ano, item.Mes, item.Dia);
                    ret.Add(new InventarioComDiferencaViewModel
                    {
                        Id = $"{ data.ToString("dd/MM/yyyy") },{ item.IdLocalArmazenamento }",
                        Nome = $"{ data.ToString("dd/MM/yyyy") } - { item.NomeLocalArmazenamento }"
                    });
                }
            }
            return ret;
        }

        public static List<ProdutoComDiferencaEmInventarioViewModel> RecuperarListaProdutoComDiferencaEmInventario(string inventario)
        {
            var ret = new List<ProdutoComDiferencaEmInventarioViewModel>();

            var data = DateTime.ParseExact(inventario.Split(',')[0], "dd/MM/yyyy", null);
            var idLocal = Int32.Parse(inventario.Split(',')[1]);

            using (var db = new ContextoBD())
            {
                ret = db.InventariosEstoques
                    .Where(x => DbFunctions.TruncateTime(x.Data) == data) // Tira a hora
                    .Where(x => x.Produto.IdLocalArmazenamento == idLocal)
                    .Where(x => x.QuantidadeEstoque > x.QuantidadeInventario)
                    .Select(x => new ProdutoComDiferencaEmInventarioViewModel
                    {
                        Id = x.Id,
                        NomeProduto = x.Produto.Nome,
                        CodigoProduto = x.Produto.Codigo,
                        QuantidadeEstoque = x.QuantidadeEstoque,
                        QuantidadeInventario = x.QuantidadeInventario,
                        Motivo = x.Motivo
                    })
                    .ToList();
            }

            return ret;
        }

        public static bool SalvarLancamentoPerda(List<LancamentoPerdaViewModel> dados)
        {
            var ret = true;

            try
            {
                using (var db = new ContextoBD())
                {
                    foreach (var lanc in dados)
                    {
                        var inventario = db.InventariosEstoques.Find(lanc.Id);
                        inventario.Motivo = lanc.Motivo;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ret = false;
            }
            return ret;
        }
        #endregion
    }
}