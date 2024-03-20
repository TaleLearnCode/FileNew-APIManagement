namespace TaleLearnCode.Quotes.Responses;

public class QuoteResponseList
{
	public int TotalCount { get; set; }
	public int PageSize { get; set; }
	public int PageCount { get; set; }
	public int PageNumber { get; set; }
	public List<QuoteResponse> Quotes { get; set; } = [];
}