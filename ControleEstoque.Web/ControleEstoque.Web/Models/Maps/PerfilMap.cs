using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class PerfilMap : EntityTypeConfiguration<PerfilModel>
    {
        public PerfilMap()
        {
            // Nome da tabela
            ToTable("perfil");

            // Chave primária e autoincremento
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome aos campos
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(50).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();

            // Associação
            HasMany(x => x.Usuarios).WithMany(x => x.Perfis)
                .Map(x =>
                {
                    x.ToTable("perfil_usuario");
                    x.MapLeftKey("id_perfil");
                    x.MapRightKey("id_usuario");
                });
        }
    }
}