namespace TaleLearnCode.Quotes.Responses;

public class QuoteResponse
{
	public int Id { get; set; }
	public string Author { get; set; } = null!;
	public string Quote { get; set; } = null!;
	public List<string> Categories { get; set; } = [];
}