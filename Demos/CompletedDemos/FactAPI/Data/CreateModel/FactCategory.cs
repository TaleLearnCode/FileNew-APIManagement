namespace TaleLearnCode.Facts.Data;

internal static partial class CreateModel
{
	internal static void FactCategory(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<FactCategory>(entity =>
		{
			entity.HasKey(e => e.FactCategoryId).HasName("pkcFactCategory");

			entity.ToTable("FactCategory");

			entity.HasOne(d => d.Category).WithMany(p => p.FactCategories)
					.HasForeignKey(d => d.CategoryId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("fkFactCategory_Category");

			entity.HasOne(d => d.Fact).WithMany(p => p.FactCategories)
					.HasForeignKey(d => d.FactId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("fkFactCategory_Fact");
		});
	}
}