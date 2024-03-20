#nullable disable

namespace TaleLearnCode.Quotes.Data.Models;

public partial class Category
{
	public int CategoryId { get; set; }

	public string CategoryName { get; set; }

	public int CategoryTypeId { get; set; }

	public virtual ICollection<QuoteCategory> QuoteCategories { get; set; } = new List<QuoteCategory>();
}