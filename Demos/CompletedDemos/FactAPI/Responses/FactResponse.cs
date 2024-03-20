namespace TaleLearnCode.Facts.Responses;

public class FactResponse
{
	public int Id { get; set; }
	public string Fact { get; set; } = null!;
	public List<string> Categories { get; set; } = [];
}