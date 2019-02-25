using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class CidadeMap : EntityTypeConfiguration<CidadeModel>
    {
        public CidadeMap()
        {
            // Nome da tabela
            ToTable("cidade");

            // Chave primária
            HasKey(x => x.Id);
            // Dando nome a coluna e autoincremento
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome a coluna 
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(30).IsRequired();

            // Dando nome a coluna 
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();

            // Dando nome a coluna 
            Property(x => x.IdEstado).HasColumnName("id_estado").IsRequired();
            // Associação ao objeto id_estado. Chave estrangeira. WillCascadeOnDelete é se
            // quando eu apagar a tabela cidade, ele deveria apagar o id_estado ou não.
            HasRequired(x => x.Estado).WithMany().HasForeignKey(x => x.IdEstado).WillCascadeOnDelete(false);
        }
    }
}