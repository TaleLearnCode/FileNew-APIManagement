#nullable disable

namespace TaleLearnCode.Facts.Data.Models;

public partial class Fact
{
	public int FactId { get; set; }

	public string Content { get; set; }

	public virtual ICollection<FactCategory> FactCategories { get; set; } = [];
}