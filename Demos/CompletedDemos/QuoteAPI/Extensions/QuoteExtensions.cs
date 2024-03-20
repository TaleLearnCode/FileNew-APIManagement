namespace TaleLearnCode.Quotes.Extensions;

internal static class QuoteExtensions
{
	internal static QuoteResponse ToResponse(this Quote quote)
		=> new()
		{
			Id = quote.QuoteId,
			Quote = quote.Content,
			Author = quote.Author,
			Categories = quote.QuoteCategories?.Select(x => x.Category.CategoryName).ToList() ?? []
		};

	internal static QuoteResponseList ToResponse(this IEnumerable<Quote> quotes, int totalCount, int pageSize, int pageCount, int pageNumber)
	=> new()
	{
		TotalCount = totalCount,
		PageSize = pageSize,
		PageNumber = pageNumber,
		PageCount = pageCount,
		Quotes = quotes.Select(x => x.ToResponse()).ToList()
	};
}