namespace TaleLearnCode.Quotes.Data;

internal static partial class CreateModel
{
	internal static void Quote(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Quote>(entity =>
		{
			entity.HasKey(e => e.QuoteId).HasName("pkcQuote");

			entity.ToTable("Quote");

			entity.Property(e => e.Author)
					.IsRequired()
					.HasMaxLength(100);
			entity.Property(e => e.Content)
					.IsRequired()
					.HasMaxLength(300);
		});
	}
}