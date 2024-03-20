#nullable disable

namespace TaleLearnCode.Facts.Data;

public class FactContext : DbContext
{

	public FactContext() { }

	public FactContext(DbContextOptions<FactContext> options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("database-connection-string")!);
	}

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Fact> Facts { get; set; }

	public virtual DbSet<FactCategory> FactCategories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		CreateModel.Category(modelBuilder);
		CreateModel.Fact(modelBuilder);
		CreateModel.FactCategory(modelBuilder);
	}

}