using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class EstadoMap : EntityTypeConfiguration<EstadoModel>
    {
        public EstadoMap()
        {
            // Nome da tabela
            ToTable("estado");

            // Chave primária e autoincremento
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome aos campos
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(30).IsRequired();
            Property(x => x.UF).HasColumnName("uf").HasMaxLength(2).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();

            // Chave estrangeira
            Property(x => x.IdPais).HasColumnName("id_pais").IsRequired();
            HasRequired(x => x.Pais).WithMany().HasForeignKey(x => x.IdPais).WillCascadeOnDelete(false);
        }
    } 
}