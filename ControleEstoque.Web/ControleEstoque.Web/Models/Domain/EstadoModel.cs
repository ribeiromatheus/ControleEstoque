using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class EstadoModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public bool Ativo { get; set; }
        public int IdPais { get; set; }
        public virtual PaisModel Pais { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;
            using (var db = new ContextoBD())
            {
                ret = db.Estados.Count();
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    ret = conexao.ExecuteScalar<int>("SELECT COUNT (*) FROM estado");
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "SELECT COUNT (*) FROM estado";

            //    ret = (int)comando.ExecuteScalar();
            //}
            //}
        }

        //private static EstadoModel MontarEstado(SqlDataReader reader)
        //{
        //    return new EstadoModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        UF = (string)reader["uf"],
        //        Ativo = (bool)reader["ativo"],
        //        IdPais = (int)reader["id_pais"]
        //    };
        //}

        public static List<EstadoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", int idPais = 0)
        {
            var ret = new List<EstadoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" WHERE LOWER(nome) LIKE '%{0}%'", filtro.ToLower());
                }

                if (idPais > 0)
                {
                    filtroWhere +=
                        (string.IsNullOrEmpty(filtroWhere) ? " WHERE" : " AND") +
                        string.Format(" id_pais = {0}", idPais);
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
                    "SELECT * FROM estado " +
                    filtroWhere +
                    " ORDER BY " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                    paginacao;
                ret = db.Database.Connection.Query<EstadoModel>(sql).ToList();
                //var reader = comando.ExecuteReader();

                //while (reader.Read())
                //{
                //    ret.Add(MontarEstado(reader));
                //}
            }

            return ret;
        }

        public static EstadoModel RecuperarPeloId(int id)
        {
            EstadoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Estados.Find(id);
                //conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                //conexao.Open();
                //var sql = "SELECT * FROM estado WHERE (id = @id)";
                //var parametros = new { id };
            }
            return ret;
            //ret = conexao.Query<EstadoModel>(sql, parametros).SingleOrDefault();
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "SELECT * FROM estado WHERE (id = @id)";

            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

            //    var reader = comando.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        ret = MontarEstado(reader);
            //    }
            //}
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var db = new ContextoBD())
                {
                    var estado = new EstadoModel { Id = id };
                    db.Estados.Attach(estado);
                    db.Entry(estado).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    var sql = "DELETE FROM estado WHERE (id = @id)";
            //    var parametros = new { id };
            //    ret = (conexao.Execute(sql, parametros) > 0);
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "DELETE FROM estado WHERE (id = @id)";

            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
            //    ret = (comando.ExecuteNonQuery() > 0);
            //}
            // }
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
                    db.Estados.Add(this);
                    //var sql = "INSERT INTO estado (nome, uf, ativo, id_pais) VALUES (@nome, @uf, @ativo, @id_pais); SELECT CONVERT(INT, SCOPE_IDENTITY())";
                    //var parametros = new { nome = this.Nome, uf = this.UF, ativo = (this.Ativo ? 1 : 0), id_pais = this.IdPais };
                    //ret = conexao.ExecuteScalar<int>(sql, parametros);
                }
                else
                {
                    db.Estados.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                    //var sql = "UPDATE estado SET nome = @nome, uf=@uf, ativo=@ativo, id_pais=@id_pais WHERE id = @id";
                    //var parametros = new { nome = this.Nome, uf = this.UF, ativo = (this.Ativo ? 1 : 0), id_pais = this.IdPais, id = this.Id };
                    //if (conexao.Execute(sql, parametros) > 0)
                    //{
                    //    ret = this.Id;
                    //}
                    //using (var comando = new SqlCommand())
                    //{
                    //    comando.Connection = conexao;

                    //    if (model == null)
                    //    {
                    //        comando.CommandText = "INSERT INTO estado (nome, uf, ativo, id_pais) VALUES (@nome, @uf, @ativo, @id_pais); SELECT CONVERT(INT, SCOPE_IDENTITY())";

                    //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                    //        comando.Parameters.Add("@uf", SqlDbType.VarChar).Value = this.UF;
                    //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                    //        comando.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.IdPais;

                    //        ret = (int)comando.ExecuteScalar();
                    //    }
                    //    else
                    //    {
                    //        comando.CommandText = "UPDATE estado SET nome = @nome, uf=@uf, ativo=@ativo, id_pais=@id_pais WHERE id = @id";

                    //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                    //        comando.Parameters.Add("@uf", SqlDbType.VarChar).Value = this.UF;
                    //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                    //        comando.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.IdPais;
                    //        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                    //        if (comando.ExecuteNonQuery() > 0)
                    //        {
                    //            ret = this.Id;
                    //        }
                    //    }
                    //}
                }
                db.SaveChanges();
                ret = this.Id;
            }
            return ret;
        }
        #endregion
    }
}