using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class LocalArmazenamentoModel
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
                ret = db.LocaisArmazenamentos.Count();
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    ret = conexao.ExecuteScalar<int>("SELECT COUNT (*) FROM tb_locaisArmazenamentos");
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "SELECT COUNT (*) FROM tb_locaisArmazenamentos";

            //    ret = (int)comando.ExecuteScalar();
            //}
            //}
        }

        //private static LocalArmazenamentoModel MontarLocalArmazenamento(SqlDataReader reader)
        //{
        //    return new LocalArmazenamentoModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Ativo = (bool)reader["ativo"]
        //    };
        //}

        public static List<LocalArmazenamentoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<LocalArmazenamentoModel>();

            using (var db = new ContextoBD())
            {
                //conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                //conexao.Open();

                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" WHERE LOWER(nome) LIKE '%{0}%'", filtro.ToLower());
                }

                var paginacao = "";
                var pos = (pagina - 1) * tamPagina;

                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",
                       pos > 0 ? pos - 1 : 0, tamPagina);
                }

                //comando.Connection = conexao;
                //comando.CommandText =
                var sql =
                    "SELECT * FROM tb_locaisArmazenamentos " +
                    filtroWhere +
                    " ORDER BY " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                    paginacao;
                ret = db.Database.Connection.Query<LocalArmazenamentoModel>(sql).ToList();
                //var reader = comando.ExecuteReader();

                //while (reader.Read())
                //{
                //    ret.Add(MontarLocalArmazenamento(reader));
                //}
            }

            return ret;
        }

        public static LocalArmazenamentoModel RecuperarPeloId(int id)
        {
            LocalArmazenamentoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.LocaisArmazenamentos.Find(id);
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
                    var localArmazenamento = new LocalArmazenamentoModel { Id = id };
                    db.LocaisArmazenamentos.Attach(localArmazenamento);
                    db.Entry(localArmazenamento).State = EntityState.Deleted;
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
                //conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                //conexao.Open();

                if (model == null)
                {
                    db.LocaisArmazenamentos.Add(this);
                    //var sql = "INSERT INTO tb_locaisArmazenamentos (nome, ativo) VALUES (@nome, @ativo); SELECT CONVERT(INT, SCOPE_IDENTITY())";
                    //var parametros = new { nome = this.Nome, ativo = (this.Ativo ? 1 : 0) };
                    //ret = conexao.ExecuteScalar<int>(sql, parametros);
                }
                else
                {
                    db.LocaisArmazenamentos.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                    //var sql = "UPDATE tb_locaisArmazenamentos SET nome = @nome, ativo=@ativo WHERE id = @id";
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
            //        comando.CommandText = "INSERT INTO tb_locaisArmazenamentos (nome, ativo) VALUES (@nome, @ativo); SELECT CONVERT(INT, SCOPE_IDENTITY())";

            //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
            //        ret = (int)comando.ExecuteScalar();
            //    }
            //    else
            //    {
            //        comando.CommandText = "UPDATE tb_locaisArmazenamentos SET nome = @nome, ativo=@ativo WHERE id = @id";

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