namespace TaleLearnCode.Facts.Requests;

public class FactRequest
{
	public string Fact { get; set; } = null!;
	public List<string> Categories { get; set; } = [];
}