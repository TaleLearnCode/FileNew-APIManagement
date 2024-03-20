#nullable disable

namespace TaleLearnCode.Quotes.Data.Models;

public partial class Quote
{
	public int QuoteId { get; set; }

	public string Author { get; set; }

	public string Content { get; set; }

	public virtual ICollection<QuoteCategory> QuoteCategories { get; set; } = [];
}