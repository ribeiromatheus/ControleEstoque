using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class FornecedorModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string NumDocumento { get; set; }
        public TipoPessoa Tipo { get; set; }
        public string Telefone { get; set; }
        public string Contato { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public int IdPais { get; set; }
        public virtual PaisModel Pais { get; set; }
        public int IdEstado { get; set; }
        public virtual EstadoModel Estado { get; set; }
        public int IdCidade { get; set; }
        public virtual CidadeModel Cidade { get; set; }
        public bool Ativo { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Fornecedores.Count();
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    ret = conexao.ExecuteScalar<int>("SELECT COUNT (*) FROM Fornecedor");
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "SELECT COUNT (*) FROM Fornecedor";

            //    ret = (int)comando.ExecuteScalar();
            //}
            //}
        }

        //private static FornecedorModel MontarFornecedor(SqlDataReader reader)
        //{
        //    return new FornecedorModel
        //    {
        //        Id = (int)reader["id"],
        //        Nome = (string)reader["nome"],
        //        RazaoSocial = (string)reader["razao_social"],
        //        NumDocumento = (string)reader["num_documento"],
        //        Tipo = (TipoPessoa)((int)reader["tipo"]),
        //        Telefone = (string)reader["telefone"],
        //        Contato = (string)reader["contato"],
        //        Logradouro = (string)reader["logradouro"],
        //        Numero = (string)reader["numero"],
        //        Complemento = (string)reader["complemento"],
        //        Cep = (string)reader["cep"],
        //        IdPais = (int)reader["id_pais"],
        //        IdEstado = (int)reader["id_estado"],
        //        IdCidade = (int)reader["id_cidade"],
        //        Ativo = (bool)reader["ativo"]
        //    };
        //}

        public static List<FornecedorModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<FornecedorModel>();

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
                    "SELECT * FROM Fornecedor" +
                    filtroWhere +
                    " ORDER BY " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                    paginacao;
                ret = db.Database.Connection.Query<FornecedorModel>(sql).ToList();
                //var reader = comando.ExecuteReader();

                //while (reader.Read())
                //{
                //    ret.Add(MontarFornecedor(reader));
                //}
            }

            return ret;
        }

        public static FornecedorModel RecuperarPeloId(int id)
        {
            FornecedorModel ret = null;
            using (var db = new ContextoBD())
            {
                ret = db.Fornecedores.Find(id);
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    var sql = "select * from Fornecedor where (id = @id)";
            //    var parametros = new { id };
            //    ret = conexao.Query<FornecedorModel>(sql, parametros).SingleOrDefault();
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "select * from Fornecedor where (id = @id)";

            //    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

            //    var reader = comando.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        ret = MontarFornecedor(reader);
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
                    var fornecedor = new FornecedorModel { Id = id };
                    db.Fornecedores.Attach(fornecedor);
                    db.Entry(fornecedor).State = EntityState.Deleted;
                    ret = true;
                }
            }
            return ret;
            //using (var conexao = new SqlConnection())
            //{
            //    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            //    conexao.Open();
            //    var sql = "DELETE FROM Fornecedor WHERE (id = @id)";
            //    var parametros = new { id };
            //    ret = (conexao.Execute(sql, parametros) > 0);
            //using (var comando = new SqlCommand())
            //{
            //    comando.Connection = conexao;
            //    comando.CommandText = "DELETE FROM Fornecedor WHERE (id = @id)";

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
                    db.Fornecedores.Add(this);
                    //var sql = "INSERT INTO Fornecedor " +
                    //    "(nome, razao_social, num_documento, tipo, " +
                    //    "telefone, contato, logradouro, numero, " +
                    //    "complemento, cep, id_pais, id_estado, " +
                    //    "id_cidade, ativo) VALUES (@nome, @razao_social, " +
                    //    "@num_documento, @tipo, @telefone, @contato, @logradouro, " +
                    //    "@numero, @complemento, @cep, @id_pais, @id_estado, @id_cidade, " +
                    //    "@ativo); SELECT CONVERT(INT, SCOPE_IDENTITY())";
                    //var parametros = new
                    //{
                    //    nome = this.Nome,
                    //    razao_social = this.RazaoSocial ?? "",
                    //    num_documento = this.NumDocumento ?? "",
                    //    tipo = this.Tipo,
                    //    telefone = this.Telefone ?? "",
                    //    contato = this.Contato ?? "",
                    //    logradouro = this.Logradouro ?? "",
                    //    numero = this.Numero ?? "",
                    //    complemento = this.Complemento ?? "",
                    //    cep = this.Cep ?? "",
                    //    id_pais = this.IdPais,
                    //    id_estado = this.IdEstado,
                    //    id_cidade = this.IdCidade,
                    //    ativo = (this.Ativo ? 1 : 0)
                    //};
                    //ret = conexao.ExecuteScalar<int>(sql, parametros);
                }
                else
                {
                    db.Fornecedores.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                    //var sql = "UPDATE Fornecedor SET nome = @nome, " +
                    //    "razao_social=@razao_social, num_documento=@num_documento, " +
                    //    "tipo=@tipo, telefone=@telefone, contato=@contato, logradouro=@logradouro, " +
                    //    "numero=@numero, complemento=@complemento, cep=@cep, " +
                    //    "id_pais=@id_pais, id_estado=@id_estado, id_cidade=@id_cidade, " +
                    //    "ativo=@ativo WHERE id = @id";
                    //var parametros = new
                    //{
                    //    nome = this.Nome,
                    //    razao_social = this.RazaoSocial ?? "",
                    //    num_documento = this.NumDocumento ?? "",
                    //    tipo = this.Tipo,
                    //    telefone = this.Telefone ?? "",
                    //    contato = this.Contato ?? "",
                    //    logradouro = this.Logradouro ?? "",
                    //    numero = this.Numero ?? "",
                    //    complemento = this.Complemento ?? "",
                    //    cep = this.Cep ?? "",
                    //    id_pais = this.IdPais,
                    //    id_estado = this.IdEstado,
                    //    id_cidade = this.IdCidade,
                    //    ativo = (this.Ativo ? 1 : 0),
                    //    id = this.Id
                    //};

                    //if (conexao.Execute(sql, parametros) > 0)
                    //{
                    //    ret = this.Id;
                    //}
                    //using (var comando = new SqlCommand())
                    //{
                    //    comando.Connection = conexao;

                    //    if (model == null)
                    //    {
                    //        comando.CommandText = "INSERT INTO Fornecedor " +
                    //            "(nome, razao_social, num_documento, tipo, " +
                    //            "telefone, contato, logradouro, numero, " +
                    //            "complemento, cep, id_pais, id_estado, " +
                    //            "id_cidade, ativo) VALUES (@nome, @razao_social, " +
                    //            "@num_documento, @tipo, @telefone, @contato, @logradouro, " +
                    //            "@numero, @complemento, @cep, @id_pais, @id_estado, @id_cidade, " +
                    //            "@ativo); SELECT CONVERT(INT, SCOPE_IDENTITY())";
                    //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                    //        comando.Parameters.Add("@razao_social", SqlDbType.VarChar).Value = this.RazaoSocial ?? "";
                    //        comando.Parameters.Add("@num_documento", SqlDbType.VarChar).Value = this.NumDocumento ?? "";
                    //        comando.Parameters.Add("@tipo", SqlDbType.Int).Value = this.Tipo;
                    //        comando.Parameters.Add("@telefone", SqlDbType.VarChar).Value = this.Telefone ?? "";
                    //        comando.Parameters.Add("@contato", SqlDbType.VarChar).Value = this.Contato ?? "";
                    //        comando.Parameters.Add("@logradouro", SqlDbType.VarChar).Value = this.Logradouro ?? "";
                    //        comando.Parameters.Add("@numero", SqlDbType.VarChar).Value = this.Numero ?? "";
                    //        comando.Parameters.Add("@complemento", SqlDbType.VarChar).Value = this.Complemento ?? "";
                    //        comando.Parameters.Add("@cep", SqlDbType.VarChar).Value = this.Cep ?? "";
                    //        comando.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.IdPais;
                    //        comando.Parameters.Add("@id_estado", SqlDbType.Int).Value = this.IdEstado;
                    //        comando.Parameters.Add("@id_cidade", SqlDbType.Int).Value = this.IdCidade;
                    //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                    //        ret = (int)comando.ExecuteScalar();
                    //    }
                    //    else
                    //    {
                    //        comando.CommandText = "UPDATE Fornecedor SET nome = @nome, " +
                    //            "razao_social=@razao_social, num_documento=@num_documento, " +
                    //            "tipo=@tipo, telefone=@telefone, contato=@contato, logradouro=@logradouro, " +
                    //            "numero=@numero, complemento=@complemento, cep=@cep, " +
                    //            "id_pais=@id_pais, id_estado=@id_estado, id_cidade=@id_cidade, " +
                    //            "ativo=@ativo WHERE id = @id";

                    //        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                    //        comando.Parameters.Add("@razao_social", SqlDbType.VarChar).Value = this.RazaoSocial ?? "";
                    //        comando.Parameters.Add("@num_documento", SqlDbType.VarChar).Value = this.NumDocumento ?? "";
                    //        comando.Parameters.Add("@tipo", SqlDbType.Int).Value = this.Tipo;
                    //        comando.Parameters.Add("@telefone", SqlDbType.VarChar).Value = this.Telefone ?? "";
                    //        comando.Parameters.Add("@contato", SqlDbType.VarChar).Value = this.Contato ?? "";
                    //        comando.Parameters.Add("@logradouro", SqlDbType.VarChar).Value = this.Logradouro ?? "";
                    //        comando.Parameters.Add("@numero", SqlDbType.VarChar).Value = this.Numero ?? "";
                    //        comando.Parameters.Add("@complemento", SqlDbType.VarChar).Value = this.Complemento ?? "";
                    //        comando.Parameters.Add("@cep", SqlDbType.VarChar).Value = this.Cep ?? "";
                    //        comando.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.IdPais;
                    //        comando.Parameters.Add("@id_estado", SqlDbType.Int).Value = this.IdEstado;
                    //        comando.Parameters.Add("@id_cidade", SqlDbType.Int).Value = this.IdCidade;
                    //        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
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