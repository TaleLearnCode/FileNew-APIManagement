#nullable disable

namespace TaleLearnCode.Facts.Data.Models;

public partial class FactCategory
{
	public int FactCategoryId { get; set; }

	public int FactId { get; set; }

	public int CategoryId { get; set; }

	public virtual Category Category { get; set; }

	public virtual Fact Fact { get; set; }
}