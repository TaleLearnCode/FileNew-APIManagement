#nullable disable

namespace TaleLearnCode.Quotes.Data.Models;

public partial class QuoteCategory
{
	public int QuoteCategoryId { get; set; }

	public int QuoteId { get; set; }

	public int CategoryId { get; set; }

	public virtual Category Category { get; set; }

	public virtual Quote Quote { get; set; }
}