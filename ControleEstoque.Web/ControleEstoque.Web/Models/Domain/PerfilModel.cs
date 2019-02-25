using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class PerfilModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public virtual List<UsuarioModel> Usuarios { get; set; }
        #endregion

        #region Métodos
        //public PerfilModel()
        //{
        //    this.Usuarios = new List<UsuarioModel>();
        //}

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Perfis.Count();
            }
            return ret;
        }

        //private static PerfilModel MontarPerfil(SqlDataReader reader)
        //{
        //    return new PerfilModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        Ativo = (bool)reader["ativo"]
        //    };
        //}

        public static List<PerfilModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<PerfilModel>();

            using (var db = new ContextoBD())
            {
                var pos = (pagina - 1) * tamPagina;

                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" WHERE LOWER(nome) LIKE '%{0}%'", filtro.ToLower());
                }

                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",
                       pos > 0 ? pos - 1 : 0, tamPagina);
                }

                var sql =
                    "select * from perfil " +
                    filtroWhere +
                    " order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                    paginacao;
                ret = db.Database.Connection.Query<PerfilModel>(sql).ToList();
            }
            return ret;
        }

        //public void CarregarUsuarios()
        //{
        //    this.Usuarios.Clear();

        //    using (var conexao = new SqlConnection())
        //    {
        //        conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
        //        conexao.Open();

        //        var sql =
        //            "select u.* " +
        //            "from perfil_usuario pu, usuario u " +
        //            "where (pu.id_perfil = @id_perfil) and (pu.id_usuario = u.id)";
        //        var parametros = new { id_perfil = this.Id };
        //        this.Usuarios = conexao.Query<UsuarioModel>(sql, parametros).ToList();
        //        //var reader = comando.ExecuteReader();
        //        //while (reader.Read())
        //        //{
        //        //    this.Usuarios.Add(new UsuarioModel
        //        //    {
        //        //        Id = (int)reader["id"],
        //        //        Nome = (string)reader["nome"],
        //        //        Login = (string)reader["login"]
        //        //    });
        //        //}
        //    }
        //}

        public static List<PerfilModel> RecuperarListaAtivos()
        {
            var ret = new List<PerfilModel>();

            using (var db = new ContextoBD())
            {
                ret = db.Perfis
                    .Where(x => x.Ativo)
                    .OrderBy(x => x.Nome)
                    .ToList();
                //using (var comando = new SqlCommand())
                //{
                //    comando.Connection = conexao;
                //    comando.CommandText = string.Format("select * from perfil where ativo=1 order by nome");
                //    var reader = comando.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        ret.Add(MontarPerfil(reader));
                //    }
                //}
            }
            return ret;
        }

        public static PerfilModel RecuperarPeloId(int id)
        {
            PerfilModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Perfis
                    .Include(x => x.Usuarios)
                    .Where(x => x.Id == id)
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
                    var perfil = new PerfilModel { Id = id };
                    db.Perfis.Attach(perfil);
                    db.Entry(perfil).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }
            return ret;
        }

        public int Salvar()
        {
            // Criar variável de retorno
            var ret = 0;

            // Criar variável para armazena o método RecuperarPeloId()
            //var model = RecuperarPeloId(this.Id);

            // Instânciar objeto para conexão com o banco de dados
            using (var db = new ContextoBD())
            {
                var model = db.Perfis
                .Include(x => x.Usuarios)
                .Where(x => x.Id == this.Id)
                .SingleOrDefault();

                /* Se o model for nulo, ou seja, não retornou nada do método
                 * RecuperarPeloId(), insere registro
                 */
                if (model == null)
                {
                    if (this.Usuarios != null && this.Usuarios.Count > 0)
                    {
                        foreach (var usuario in this.Usuarios)
                        {
                            db.Usuarios.Attach(usuario);
                            db.Entry(usuario).State = EntityState.Unchanged;
                        }
                    }
                    db.Perfis.Add(this);
                }
                // Senão, altera o registro
                else
                {
                    model.Nome = this.Nome;
                    model.Ativo = this.Ativo;

                    foreach (var usuario in model.Usuarios.FindAll(x => !this.Usuarios.Exists(u => u.Id == x.Id)))
                    {
                        model.Usuarios.Remove(usuario);
                    }
                    foreach (var usuario in this.Usuarios.FindAll(x => x.Id > 0 && !model.Usuarios.Exists(u => u.Id == x.Id)))
                    {
                        db.Usuarios.Attach(usuario);
                        db.Entry(usuario).State = EntityState.Unchanged;
                        model.Usuarios.Add(usuario);
                    }
                    //db.Perfis.Attach(this);
                    //db.Entry(this).State = EntityState.Modified;
                }
                // Salva as alterações feitas
                db.SaveChanges();
                ret = this.Id;
                
                // Se a lista de usuários não forem nulas e a contagem maior que 0 (zero)
                //if (this.Usuarios != null && this.Usuarios.Count > 0)
                //{
                //    // Passar a instrução SQL
                //    var sql = "delete from perfil_usuario where (id_perfil = @id_perfil)";

                //    // Passar o parâmetro
                //    var parametros = new { id_perfil = this.Id };

                //    // Executa a query
                //    db.Database.Connection.Execute(sql, parametros, transacao);

                //    // Se o primeiro usuário que vier na lista não tiver o ID igual a -1
                //    if (this.Usuarios[0].Id != -1)
                //    {
                //        /* Laço de repetição foreach
                //         * Criar variável para armazenar o que vier na lista de usuários
                //         */
                //        foreach (var usuario in this.Usuarios)
                //        {
                //            // Passar a instrução SQL
                //            sql = "insert into perfil_usuario (id_perfil, id_usuario) values (@id_perfil, @id_usuario)";

                //            // Passar os parâmetros
                //            var parametrosUsuarios = new { id_perfil = this.Id, id_usuario = usuario.Id };

                //            // Executa a query
                //            db.Database.Connection.Execute(sql, parametrosUsuarios, transacao);
                //        }
                //    }
                //}
            }
            // Retorna o que vier na variável de retorno
            return ret;

            //if (model == null)
            //{
            //    // Passar a instrução SQL
            //    comando.CommandText = "insert into perfil (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity())";

            //    // Passar os parâmetros
            //    comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //    comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);

            //    // Passar para a variável de retorno um valor escalar
            //    ret = (int)comando.ExecuteScalar();

            //    // Popula o ID com esse valor
            //    this.Id = ret;
            //}
            //// Senão, altera o registro
            //else
            //{
            //    // Passar a instrução SQL
            //    comando.CommandText = "update perfil set nome=@nome, ativo=@ativo where id = @id";

            //    // Passar os parâmetros
            //    comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
            //    comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

            //    // Se o número de linha retornada é maior que 0 (zero)
            //    if (comando.ExecuteNonQuery() > 0)
            //    {
            //        // Popula a variável de retorno com o ID
            //        ret = this.Id;
            //    }
            //}

            //         Se a lista de usuários não forem nulas e a contagem maior que 0 (zero)
            //            if (this.Usuarios != null && this.Usuarios.Count > 0)
            //            {
            //    Instânciar objeto de comando para exclusão do perfil de usuário
            //    using (var comandoExclusaoPerfilUsuario = new SqlCommand())
            //    {
            //        Pasar a conexão
            //        comandoExclusaoPerfilUsuario.Connection = conexao;

            //        Passar a transação
            //        comandoExclusaoPerfilUsuario.Transaction = transacao;

            //        Passar a instrução SQL
            //        comandoExclusaoPerfilUsuario.CommandText = "delete from perfil_usuario where (id_perfil = @id_perfil)";

            //        Passar o parâmetro
            //        comandoExclusaoPerfilUsuario.Parameters.Add("@id_perfil", SqlDbType.Int).Value = this.Id;

            //        Executa a query retornando valor escalar
            //        comandoExclusaoPerfilUsuario.ExecuteScalar();
            //    }

            //    Se o primeiro usuário que vier na lista não tiver o ID igual a -1
            //    if (this.Usuarios[0].Id != -1)
            //    {
            //        /* Laço de repetição foreach
            //         * Criar variável para armazenar o que vier na lista de usuários
            //         */
            //        foreach (var usuario in this.Usuarios)
            //        {
            //            Instânciar objeto de comando para inclusão de perfil de usuário
            //            using (var usuarioInclusaoPerfilUsuario = new SqlCommand())
            //            {
            //                Passar a conexão
            //                usuarioInclusaoPerfilUsuario.Connection = conexao;

            //                Passar a transação
            //                usuarioInclusaoPerfilUsuario.Transaction = transacao;

            //                Passar a instrução SQL
            //                usuarioInclusaoPerfilUsuario.CommandText = "insert into perfil_usuario (id_perfil, id_usuario) values (@id_perfil, @id_usuario)";

            //                Passar os parâmetros
            //                usuarioInclusaoPerfilUsuario.Parameters.Add("@id_perfil", SqlDbType.Int).Value = this.Id;
            //                usuarioInclusaoPerfilUsuario.Parameters.Add("@id_usuario", SqlDbType.Int).Value = usuario.Id;

            //                Executa a query retornando valor escalar
            //                usuarioInclusaoPerfilUsuario.ExecuteScalar();
            //            }
            //        }
            //    }
            //}

            //Salva as alterações feitas
            //transacao.Commit();
            //}
            //}

            //             Retorna o que vier na variável de retorno
            //            return ret;
            //        }
            //}
            //}
        }
        #endregion
    }
}