WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new() { Title = "Random Quotes API", Description = "Provides random quotes.", Version = "v1" });
});

WebApplication app = builder.Build();

app.Use(async (context, next) =>
{
	context.Response.Headers.Append("X-Developed-By", "TaleLearnCode");
	await next.Invoke();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", GetQuotesAsync)
	.WithName("GetQuotes")
	.WithDisplayName("Get Quotes")
	.WithSummary("Retrieves the list of quotes")
	.WithDescription("Retrieves the list of quotes")
	.WithOpenApi().CacheOutput();

app.MapPost("/", CreateQuoteAsync)
	.WithName("AddQuote")
	.WithDisplayName("Add Quote")
	.WithSummary("Adds a quote to the database.")
	.WithDescription("Adds a quote to the database.");

app.MapGet("/{id}", GetQuoteAsync)
	.WithName("GetQuote")
	.WithDisplayName("Get Quote by Id")
	.WithSummary("Retrieves the quote specified by its identifier.")
	.WithDescription("Retrieves the quote specified by its identifier")
	.WithMetadata("id", "The identifier of the quote to retrieve.")
	.WithOpenApi();

app.MapPut("/{id}", UpdateAsync)
	.WithName("UpdateQuote")
	.WithDisplayName("Update Quote")
	.WithSummary("Updates the quote specified by its identifier.")
	.WithDescription("Updates the quote specified by its identifier.")
	.WithMetadata("id", "The identifier of the quote to update.")
	.WithOpenApi();

app.MapDelete("/{id}", DeleteAsync)
	.WithName("DeleteQuote")
	.WithDisplayName("Delete Quote")
	.WithSummary("Deletes the quote specified by its identifier.")
	.WithDescription("Deletes the quote specified by its identifier.")
	.WithMetadata("id", "The identifier of the quote to delete.")
	.WithOpenApi();

app.MapGet("/categories", GetCategoriesAsync)
	.WithName("GetCategories")
	.WithDisplayName("Get Categories")
	.WithSummary("Retrieves the list of quote categories.")
	.WithDescription("Retrieves the list of quote categories.")
	.WithOpenApi();

app.MapGet("/categories/{category}", GetQuotesForCategoryAsync)
	.WithName("GetQuotesForCategory")
	.WithDisplayName("Get Quotes for Category")
	.WithSummary("Retrieves the quotes for the specified category.")
	.WithDescription("Retrieves the quotes for the specified category.")
	.WithMetadata("category", "The category of the quotes to retrieve.")
	.WithOpenApi();

app.MapGet("/authors", GetAuthorsAsync)
	.WithName("GetAuthors")
	.WithDisplayName("Get Authors")
	.WithSummary("Retrieves the list of quote authors.")
	.WithDescription("Retrieves the list of quote authors.")
	.WithOpenApi();

app.MapGet("/authors/{author}", GetQuotesForAuthorAsync)
	.WithName("GetQuotesForAuthor")
	.WithDisplayName("Get Quotes for Author")
	.WithSummary("Retrieves the quotes for the specified author.")
	.WithDescription("Retrieves the quotes for the specified author.")
	.WithMetadata("author", "The author of the quotes to retrieve.")
	.WithOpenApi();

app.Run();

static async Task<IResult> GetQuotesAsync(int pageIndex = 1, int pageSize = 10)
{
	QuoteContext context = new();
	int totalCount = await context.Quotes.CountAsync();
	int pageCount = (totalCount > 0) ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
	List<Quote> quotes = await context.Quotes
		.Include(x => x.QuoteCategories)
			.ThenInclude(x => x.Category)
		.Skip((pageIndex - 1) * pageSize)
		.Take(pageSize)
		.ToListAsync();
	return TypedResults.Ok(quotes.ToResponse(totalCount, pageSize, pageIndex, pageCount));
}

static async Task<IResult> GetQuoteAsync(int id)
{
	Quote? quote = await RetrieveQuoteAsync(id);
	return quote is not null
		? TypedResults.Ok(quote.ToResponse())
		: TypedResults.NotFound();
}

static async Task<IResult> CreateQuoteAsync(QuoteRequest quoteRequest)
{
	QuoteContext context = new();
	Quote quote = quoteRequest.ToQuote();
	context.Quotes.Add(quote);
	await context.SaveChangesAsync();
	await AddCategoriesToQuoteAsync(context, quote, quoteRequest.Categories);
	return TypedResults.Created($"/{quote.QuoteId}");
}

static async Task<IResult> GetCategoriesAsync()
{
	QuoteContext context = new();
	return TypedResults.Ok(await context.Categories.Where(x => x.CategoryTypeId == StaticValues.QuoteCategoryTypeId).Select(x => x.CategoryName).ToListAsync());
}

static async Task<IResult> GetQuotesForCategoryAsync(string category, int pageIndex = 1, int pageSize = 10)
{
	QuoteContext context = new();
	int totalCount = await context.Quotes
		.Include(x => x.QuoteCategories)
			.ThenInclude(x => x.Category)
		.Where(x => x.QuoteCategories.Any(x => x.Category.CategoryName == category))
		.CountAsync();
	int pageCount = (totalCount > 0) ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
	List<Quote> quotes = await context.Quotes
		.Include(x => x.QuoteCategories)
			.ThenInclude(x => x.Category)
		.Where(x => x.QuoteCategories.Any(x => x.Category.CategoryName == category))
		.Skip((pageIndex - 1) * pageSize)
		.Take(pageSize)
		.ToListAsync();
	return TypedResults.Ok(quotes.ToResponse(totalCount, pageSize, pageIndex, pageCount));
}

static async Task<IResult> GetAuthorsAsync()
{
	QuoteContext context = new();
	return TypedResults.Ok(await context.Quotes.Select(x => x.Author).Distinct().ToListAsync());
}

static async Task<IResult> GetQuotesForAuthorAsync(string author, int pageIndex = 1, int pageSize = 10)
{
	QuoteContext context = new();
	int totalCount = await context.Quotes
		.Where(x => x.Author == author)
		.CountAsync();
	int pageCount = (totalCount > 0) ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
	List<Quote> quotes = await context.Quotes
		.Where(x => x.Author == author)
		.Skip((pageIndex - 1) * pageSize)
		.Take(pageSize)
		.ToListAsync();
	return TypedResults.Ok(quotes.ToResponse(totalCount, pageSize, pageIndex, pageCount));
}

static async Task<IResult> UpdateAsync(int id, QuoteRequest quoteRequest)
{
	QuoteContext context = new();
	Quote? quote = await RetrieveQuoteAsync(id, context);
	if (quote is null) return TypedResults.NotFound("Quote not found");
	quote.Content = quoteRequest.Quote;
	quote.Author = quoteRequest.Author;
	context.Quotes.Update(quote);
	await context.SaveChangesAsync();
	await RemoveCategoriesAsync(context, quote);
	await AddCategoriesToQuoteAsync(context, quote, quoteRequest.Categories);
	return TypedResults.NoContent();
}

static async Task<IResult> DeleteAsync(int id)
{
	QuoteContext context = new();
	Quote? quote = await RetrieveQuoteAsync(id, context);
	if (quote is null) return TypedResults.NotFound("Quote not found");
	await RemoveCategoriesAsync(context, quote);
	context.Quotes.Remove(quote);
	await context.SaveChangesAsync();
	return TypedResults.Ok();
}

static async Task<Quote?> RetrieveQuoteAsync(int id, QuoteContext? context = null)
{
	context ??= new();
	return await context.Quotes
		.Include(x => x.QuoteCategories)
			.ThenInclude(x => x.Category)
		.FirstOrDefaultAsync(x => x.QuoteId == id);
}

static async Task AddCategoriesToQuoteAsync(QuoteContext context, Quote quote, IEnumerable<string> categoryNames)
{
	foreach (string categoryName in categoryNames)
	{
		Category? category = await context.Categories
			.FirstOrDefaultAsync(x => x.CategoryName == categoryName);
		if (category is null)
		{
			category = new()
			{
				CategoryName = categoryName,
				CategoryTypeId = StaticValues.QuoteCategoryTypeId
			};
			context.Categories.Add(category);
		}
		quote.QuoteCategories.Add(new() { Category = category });
	}
	await context.SaveChangesAsync();
}

static async Task RemoveCategoriesAsync(QuoteContext context, Quote quote)
{
	List<Category> categories = quote.QuoteCategories.Select(x => x.Category).ToList();
	context.RemoveRange(quote.QuoteCategories);
	await context.SaveChangesAsync();
	foreach (Category category in categories)
		if ((await context.QuoteCategories.CountAsync(x => x.CategoryId == category.CategoryId)) == 0)
			context.Categories.Remove(category);
	await context.SaveChangesAsync();
}