#nullable disable

namespace TaleLearnCode.Quotes.Data;

public class QuoteContext : DbContext
{

	public QuoteContext() { }

	public QuoteContext(DbContextOptions<QuoteContext> options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("database-connection-string")!);
	}

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Quote> Quotes { get; set; }

	public virtual DbSet<QuoteCategory> QuoteCategories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		CreateModel.Category(modelBuilder);
		CreateModel.Quote(modelBuilder);
		CreateModel.QuoteCategory(modelBuilder);
	}

}