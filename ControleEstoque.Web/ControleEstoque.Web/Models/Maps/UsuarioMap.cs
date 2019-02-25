using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class UsuarioMap : EntityTypeConfiguration<UsuarioModel>
    {
        public UsuarioMap()
        {
            // Nome da tabela
            ToTable("usuario");

            // Chave primária e autoincremento
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome aos campos
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(50).IsRequired();
            Property(x => x.Login).HasColumnName("login").HasMaxLength(15).IsRequired();
            Property(x => x.Senha).HasColumnName("senha").HasMaxLength(50).IsRequired();
            Property(x => x.Email).HasColumnName("email").HasMaxLength(150).IsRequired();

        }
    }
}