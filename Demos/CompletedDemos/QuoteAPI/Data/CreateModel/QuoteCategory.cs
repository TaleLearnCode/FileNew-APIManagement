namespace TaleLearnCode.Quotes.Data;

internal static partial class CreateModel
{
	internal static void QuoteCategory(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<QuoteCategory>(entity =>
		{
			entity.HasKey(e => e.QuoteCategoryId).HasName("pkcQuoteCategory");

			entity.ToTable("QuoteCategory");

			entity.HasOne(d => d.Category).WithMany(p => p.QuoteCategories)
					.HasForeignKey(d => d.CategoryId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("fkQuoteCategory_Category");

			entity.HasOne(d => d.Quote).WithMany(p => p.QuoteCategories)
					.HasForeignKey(d => d.QuoteId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("fkQuoteCategory_Quote");
		});
	}
}