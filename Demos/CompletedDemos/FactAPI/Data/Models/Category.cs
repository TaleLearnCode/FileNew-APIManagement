#nullable disable

namespace TaleLearnCode.Facts.Data.Models;

public partial class Category
{
	public int CategoryId { get; set; }

	public string CategoryName { get; set; }

	public int CategoryTypeId { get; set; }

	public virtual ICollection<FactCategory> FactCategories { get; set; } = new List<FactCategory>();
}