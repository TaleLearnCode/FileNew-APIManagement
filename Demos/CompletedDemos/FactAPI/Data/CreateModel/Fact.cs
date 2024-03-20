namespace TaleLearnCode.Facts.Data;

internal static partial class CreateModel
{
	internal static void Fact(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Fact>(entity =>
		{
			entity.HasKey(e => e.FactId).HasName("pkcFact");

			entity.ToTable("Fact");

			entity.Property(e => e.Content)
					.IsRequired()
					.HasMaxLength(200);
		});
	}
}