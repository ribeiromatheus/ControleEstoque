using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class GrupoProdutoModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;
            using (var db = new ContextoBD())
            {
                ret = db.GruposProdutos.Count();
            }
            return ret;

            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    ret = conexao.ExecuteScalar<int>("SELECT COUNT (*) FROM tb_grupoProdutos");
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "SELECT COUNT (*) FROM tb_grupoProdutos";

            //    ret = (int)comando.ExecuteScalar();
            //}
            //}
        }

        //private static GrupoProdutoModel MontarGrupoProduto(SqlDataReader reader)
        //{
        //    return new GrupoProdutoModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Ativo = (bool)reader["ativo"]
        //    };
        //}

        public static List<GrupoProdutoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<GrupoProdutoModel>();

            using (var db = new ContextoBD())
            {
                //conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                //conexao.Open();

                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" WHERE LOWER(nome) LIKE '%{0}%'", filtro.ToLower());
                }

                var pos = (pagina - 1) * tamPagina;

                //comando.Connection = conexao;
                //comando.CommandText =
                var sql = string.Format(
                    "SELECT * FROM tb_grupoProdutos " +
                    filtroWhere +
                    " ORDER BY " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                    " OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",
                    pos, tamPagina);

                ret = db.Database.Connection.Query<GrupoProdutoModel>(sql).ToList();
                //var reader = comando.ExecuteReader();

                //while (reader.Read())
                //{
                //    ret.Add(MontarGrupoProduto(reader));
                //}
            }

            return ret;
        }

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.GruposProdutos.Find(id);
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    var sql = "SELECT * FROM tb_grupoProdutos WHERE (id = @id)";
            //    var parametros = new { id };
            //    ret = conexao.Query<GrupoProdutoModel>(sql, parametros).SingleOrDefault();
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "SELECT * FROM tb_grupoProdutos WHERE (id = @id)";

            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

            //    var reader = comando.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        ret = MontarGrupoProduto(reader);
            //    }
            //}
            //}
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var db = new ContextoBD())
                {
                    var grupoProduto = new GrupoProdutoModel { Id = id };
                    db.GruposProdutos.Attach(grupoProduto);
                    db.Entry(grupoProduto).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    var sql = "DELETE FROM tb_grupoProdutos WHERE (id = @id)";
            //    var parametros = new { id };
            //    ret = (conexao.Execute(sql, parametros) > 0);
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "DELETE FROM tb_grupoProdutos WHERE (id = @id)";

            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
            //    ret = (comando.ExecuteNonQuery() > 0);
            //}
            //}
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var db = new ContextoBD())
            {
                //conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                //conexao.Open();

                if (model == null)
                {
                    db.GruposProdutos.Add(this);
                    //var sql = "INSERT INTO tb_grupoProdutos (nome, ativo) VALUES (@nome, @ativo); SELECT CONVERT(INT, SCOPE_IDENTITY())";
                    //var parametros = new { nome = this.Nome, ativo = (this.Ativo ? 1 : 0) };
                    //ret = conexao.ExecuteScalar<int>(sql, parametros);
                }
                else
                {
                    db.GruposProdutos.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                    //var sql = "UPDATE tb_grupoProdutos SET nome = @nome, ativo=@ativo WHERE id = @id";
                    //var parametros = new { nome = this.Nome, ativo = (this.Ativo ? 1 : 0), id = this.Id };
                    //if (conexao.Execute(sql, parametros) > 0)
                    //{
                    //    ret = this.Id;
                    //}
                }
                db.SaveChanges();
                ret = this.Id;
            }
            return ret;
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;

            //    if (model == null)
            //    {
            //        comando.CommandText = "INSERT INTO tb_grupoProdutos (nome, ativo) VALUES (@nome, @ativo); SELECT CONVERT(INT, SCOPE_IDENTITY())";

            //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
            //        ret = (int)comando.ExecuteScalar();
            //    }
            //    else
            //    {
            //        comando.CommandText = "UPDATE tb_grupoProdutos SET nome = @nome, ativo=@ativo WHERE id = @id";

            //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
            //        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
            //        if (comando.ExecuteNonQuery() > 0)
            //        {
            //            ret = this.Id;
            //        }
            //    }
            //}
        }
        #endregion
    }
}