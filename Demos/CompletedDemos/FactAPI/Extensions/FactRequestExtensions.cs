namespace TaleLearnCode.Facts.Extensions;

public static class FactRequestExtensions
{

	public static Fact ToFact(this FactRequest request)
		=> new()
		{
			Content = request.Fact
		};

}