using Dapper;
using ControleEstoque.Web.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public virtual List<PerfilModel> Perfis { get; set; }
        #endregion

        #region Métodos
        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel ret = null;
            senha = CriptoHelper.HashMD5(senha);

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios
                    .Include(x => x.Perfis)
                    .Where(x => x.Login == login && x.Senha == senha)
                    .SingleOrDefault();
            }
            return ret;
        }

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios.Count();
            }
            return ret;
        }

        //private static UsuarioModel MontarUsuario(SqlDataReader reader)
        //{
        //    return new UsuarioModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Login = (string)reader["login"],
        //        Email = (string)reader["email"]
        //    };
        //}

        public static List<UsuarioModel> RecuperarLista(int pagina = -1, int tamPagina = -1, string filtro = "", string ordem = "")
        {
            var ret = new List<UsuarioModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" WHERE LOWER(nome) LIKE '%{0}%'", filtro.ToLower());
                }

                string sql;
                if (pagina == -1 || tamPagina == -1)
                {
                    sql = "SELECT * FROM Usuario" + filtroWhere + " ORDER BY " +
                        (!string.IsNullOrEmpty(ordem) ? ordem : "nome");
                }
                else
                {
                    var pos = (pagina - 1) * tamPagina;
                    sql = string.Format(
                        "SELECT * FROM Usuario " +
                        filtroWhere +
                        " ORDER BY " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                        " OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                }
                ret = db.Database.Connection.Query<UsuarioModel>(sql).ToList();
            }
            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios.Find(id);
            }
            return ret;
        }

        public static UsuarioModel RecuperarPeloLogin(string login)
        {
            UsuarioModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios
                    .Where(x => x.Login == login)
                   .SingleOrDefault();
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
                    var usuario = new UsuarioModel { Id = id };
                    db.Usuarios.Attach(usuario);
                    db.Entry(usuario).State = EntityState.Deleted;
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
                    if (!string.IsNullOrEmpty(this.Senha))
                    {
                        this.Senha = CriptoHelper.HashMD5(this.Senha);
                    }
                    db.Usuarios.Add(this);
                    //var sql = "INSERT INTO Usuario (nome, login, senha, email) VALUES (@nome, @login, @senha, @email); SELECT CONVERT(INT, SCOPE_IDENTITY())";
                    //var parametros = new
                    //{
                    //    nome = this.Nome,
                    //    login = this.Login,
                    //    senha = CriptoHelper.HashMD5(this.Senha),
                    //    email = this.Email
                    //};
                    //ret = db.Database.Connection.ExecuteScalar<int>(sql, parametros);
                }
                else
                {
                    db.Usuarios.Attach(this);
                    db.Entry(this).State = EntityState.Modified;

                    if (string.IsNullOrEmpty(this.Senha))
                    {
                        db.Entry(this).Property(x => x.Senha).IsModified = false;
                    }
                    else
                    {
                        this.Senha = CriptoHelper.HashMD5(this.Senha);
                    }
                    //if (!string.IsNullOrEmpty(this.Senha))
                    //{
                    //    var sql =
                    //    "UPDATE Usuario SET nome = @nome, login = @login, email = @email" +
                    //    ", senha = @senha" +
                    //    " WHERE id = @id";

                    //    var parametros = new
                    //    {
                    //        nome = this.Nome,
                    //        login = this.Login,
                    //        senha = CriptoHelper.HashMD5(this.Senha),
                    //        email = this.Email
                    //    };
                    //    if (db.Database.Connection.Execute(sql, parametros) > 0)
                    //    {
                    //        ret = this.Id;
                    //    }
                    //}
                    //else
                    //{
                    //    var sql =
                    //    "UPDATE Usuario SET nome = @nome, login = @login, email = @email" +
                    //    " WHERE id = @id";

                    //    var parametros = new
                    //    {
                    //        nome = this.Nome,
                    //        login = this.Login,
                    //        senha = CriptoHelper.HashMD5(this.Senha),
                    //        email = this.Email
                    //    };
                    //    if (db.Database.Connection.Execute(sql, parametros) > 0)
                    //    {
                    //        ret = this.Id;
                    //    }
                    //}
                }
                db.SaveChanges();
                ret = this.Id;
                //using (var comando = new SqlCommand())
                //{
                //    comando.Connection = conexao;

                //    if (model == null)
                //    {
                //        comando.CommandText = "INSERT INTO Usuario (nome, login, senha, email) VALUES (@nome, @login, @senha, @email); SELECT CONVERT(INT, SCOPE_IDENTITY())";

                //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                //        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                //        comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                //        comando.Parameters.Add("@email", SqlDbType.VarChar).Value = this.Email;
                //        ret = (int)comando.ExecuteScalar();
                //    }
                //    else
                //    {
                //        comando.CommandText =
                //            "UPDATE Usuario SET nome = @nome, login = @login, email=@email" +
                //            (!string.IsNullOrEmpty(this.Senha) ? ", senha = @senha" : "") +
                //            " WHERE id = @id";

                //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                //        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                //        comando.Parameters.Add("@email", SqlDbType.VarChar).Value = this.Email;

                //        if (!string.IsNullOrEmpty(this.Senha))
                //        {
                //            comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                //        }

                //        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                //        if (comando.ExecuteNonQuery() > 0)
                //        {
                //            ret = this.Id;
                //        }
                //    }
                //}
            }
            return ret;
        }

        public string RecuperarStringNomePerfis()
        {
            var ret = string.Empty;

            if (this.Perfis != null && this.Perfis.Count > 0)
            {
                var perfis = this.Perfis.Select(x => x.Nome);
                ret = string.Join(";", perfis);
            }
            return ret;
            //using (var db = new ContextoBD())
            //{
            //    var sql =
            //    "select p.nome " +
            //    "from perfil_usuario pu, perfil p " +
            //    "where (pu.id_usuario = @id_usuario) and (pu.id_perfil = p.id) and (p.ativo = 1)";
            //    var parametros = new { id_usuario = this.Id };
            //    var matriculas = db.Database.Connection.Query<string>(sql, parametros).ToList();
            //    if (matriculas.Count > 0)
            //    {
            //        ret = string.Join(";", matriculas);
            //    }
            //comando.Parameters.Add("@id_usuario", SqlDbType.Int).Value = this.Id;
            //var reader = comando.ExecuteReader();

            //while (reader.Read())
            //{
            //    ret += (ret != string.Empty ? ";" : string.Empty) + (string)reader["nome"];
            //}
            //}
        }

        public bool AlterarSenha(string novaSenha)
        {
            var ret = false;

            using (var db = new ContextoBD())
            {
                this.Senha = CriptoHelper.HashMD5(novaSenha);
                db.Usuarios.Attach(this);
                db.Entry(this).Property(x => x.Senha).IsModified = true;
                db.SaveChanges();
                //var sql = "UPDATE usuario set senha = @senha WHERE id = @id";
                //var parametros = new { id = this.Id, senha = CriptoHelper.HashMD5(novaSenha) };
                //ret = (db.Database.Connection.Execute(sql, parametros) > 0);
                //using (var comando = new SqlCommand())
                //{
                //    comando.Connection = conexao;
                //    comando.CommandText = "UPDATE usuario set senha = @senha WHERE id = @id";

                //    comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                //    comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(novaSenha);

                //    ret = (comando.ExecuteNonQuery() > 0);
                //}
            }
            return ret;
        }

        public bool ValidarSenhaAtual(string senhaAtual)
        {
            var ret = false;

            using (var db = new ContextoBD())
            {
                db.Usuarios
                    .Where(x => x.Senha == senhaAtual && x.Id == this.Id)
                    .Any();
                //var sql = "SELECT COUNT (*) FROM usuario WHERE senha = @senhaAtual AND id = @id";
                //var parametros = new { id = this.Id, senha_atual = CriptoHelper.HashMD5(senhaAtual) };
                //ret = (db.Database.Connection.ExecuteScalar<int>(sql, parametros) > 0);
                //using (var comando = new SqlCommand())
                //{
                //    comando.Connection = conexao;
                //    comando.CommandText = "SELECT COUNT (*) FROM usuario WHERE senha = @senhaAtual AND id = @id";

                //    comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                //    comando.Parameters.Add("@senhaAtual", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senhaAtual);

                //    ret = ((int)comando.ExecuteScalar() > 0);
                //}
            }
            return ret;
        }
        #endregion
    }
}