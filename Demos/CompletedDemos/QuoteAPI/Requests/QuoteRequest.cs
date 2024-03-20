namespace TaleLearnCode.Quotes.Requests;

public class QuoteRequest
{
	public string Author { get; set; } = null!;
	public string Quote { get; set; } = null!;
	public List<string> Categories { get; set; } = [];
}
