using TaleLearnCode.Quotes.Data.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new() { Title = "Random Facts API", Description = "Provides random facts.", Version = "v1" });
});

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", GetFactsAsync)
	.WithName("GetFacts")
	.WithSummary("GetFacts")
	.WithDescription("Retrieves the list of facts")
	.WithMetadata("pageIndex", "The page number to retrieve")
	.WithMetadata("pageSize", "The number of items to retrieve")
	.WithOpenApi()
	.CacheOutput();

app.MapPost("/", CreateFactAsync)
	.WithName("AddFact")
	.WithSummary("AddFact")
	.WithDescription("Adds a fact to the database.")
	.WithMetadata("request", "Details of the fact to be added.")
	.WithOpenApi();

app.MapGet("/{id}", GetFactAsync)
	.WithName("GetFact")
	.WithSummary("GetFact")
	.WithDescription("Retrieves the specified fact.")
	.WithMetadata("id", "Identifier of the fact to retrieve.")
	.WithOpenApi()
	.CacheOutput();

app.MapPut("/{id}", UpdateFactAsync)
	.WithName("UpdateFact")
	.WithSummary("UpdateFact")
	.WithDescription("Updates the specified fact.")
	.WithMetadata("id", "Identifier of the fact to update.")
	.WithMetadata("request", "Details of the fact to be updated.")
	.WithOpenApi();

app.MapDelete("/{id}", DeleteFactAsync)
	.WithName("DeleteFact")
	.WithSummary("DeleteFact")
	.WithDescription("Deletes the specified fact.")
	.WithMetadata("id", "Identifier of the fact to delete.")
	.WithOpenApi();

app.MapGet("/categories", GetCategoriesAsync)
	.WithName("GetCategories")
	.WithSummary("GetCategories")
	.WithDescription("Retrieves the list of fact categories.")
	.WithOpenApi()
	.CacheOutput();

app.MapGet("/categories/{category}", GetFactsForCategory)
	.WithName("GetFactsForCategory")
	.WithSummary("GetFactsForCategory")
	.WithDescription("Retrieves the list of facts for the specified category.")
	.WithMetadata("category", "The category of facts to retrieve.")
	.WithMetadata("pageIndex", "The page number to retrieve")
	.WithMetadata("pageSize", "The number of items to retrieve")
	.WithOpenApi()
	.CacheOutput();

static async Task<IResult> GetFactAsync(int id)
{
	Fact? fact = await RetrieveFactAsync(id);
	return fact is not null
		? TypedResults.Ok(fact.ToResponse())
		: TypedResults.NotFound();
}

static async Task<IResult> GetFactsAsync(int pageIndex = 1, int pageSize = 10)
{
	FactContext context = new();
	int totalCount = await context.Facts.CountAsync();
	int pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
	List<Fact> facts = await context.Facts
		.Include(x => x.FactCategories)
			.ThenInclude(x => x.Category)
		.Skip((pageIndex - 1) * pageSize)
		.Take(pageSize)
		.ToListAsync();
	return TypedResults.Ok(facts.ToResponse(totalCount, pageSize, pageCount, pageIndex));
}

static async Task<IResult> CreateFactAsync(FactRequest request)
{
	FactContext context = new();
	Fact factContent = request.ToFact();
	context.Facts.Add(factContent);
	await context.SaveChangesAsync();

	await AddCategoriesToFactAsync(context, factContent, request.Categories);

	return TypedResults.Created($"/{factContent.FactId}");
}

static async Task<IResult> UpdateFactAsync(int id, FactRequest request)
{
	FactContext context = new();
	Fact? fact = await RetrieveFactAsync(id, context);
	if (fact is null) return TypedResults.NotFound("Fact not found");
	fact.Content = request.Fact;
	context.Facts.Update(fact);
	await context.SaveChangesAsync();
	await RemoveCategoriesAsync(context, fact);
	await AddCategoriesToFactAsync(context, fact, request.Categories);
	return TypedResults.NoContent();
}

static async Task<IResult> DeleteFactAsync(int id)
{
	FactContext context = new();
	Fact? fact = await context.Facts
		.Include(x => x.FactCategories)
		.FirstOrDefaultAsync(x => x.FactId == id);
	if (fact is null) return TypedResults.NotFound("Fact not found");
	await RemoveCategoriesAsync(context, fact);
	context.Facts.Remove(fact);
	await context.SaveChangesAsync();
	return TypedResults.Ok();
}

static async Task<IResult> GetCategoriesAsync()
{
	FactContext context = new();
	return TypedResults.Ok(await context.Categories
		.Where(x => x.CategoryTypeId == StaticValues.QuoteCategoryTypeId)
		.Select(x => x.CategoryName).ToListAsync());
}

static async Task<IResult> GetFactsForCategory(string category, int pageIndex = 1, int pageSize = 10)
{
	FactContext context = new();
	int totalCount = await context.Facts
		.Include(x => x.FactCategories)
			.ThenInclude(x => x.Category)
		.Where(x => x.FactCategories.Any(x => x.Category.CategoryName == category))
		.CountAsync();
	int pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
	List<Fact> facts = await context.Facts
		.Include(x => x.FactCategories)
			.ThenInclude(x => x.Category)
		.Where(x => x.FactCategories.Any(x => x.Category.CategoryName == category))
		.Skip((pageIndex - 1) * pageSize)
		.Take(pageSize)
		.ToListAsync();
	return TypedResults.Ok(facts.ToResponse(totalCount, pageSize, pageCount, pageIndex));
}

static async Task<Fact?> RetrieveFactAsync(int id, FactContext? context = null)
{
	context ??= new();
	return await context.Facts
		.Include(x => x.FactCategories)
			.ThenInclude(x => x.Category)
		.FirstOrDefaultAsync(x => x.FactId == id);
}

static async Task AddCategoriesToFactAsync(FactContext context, Fact fact, List<string> categories)
{
	foreach (string category in categories)
	{
		Category? factCategory = await context.Categories.FirstOrDefaultAsync(x => x.CategoryName == category);
		if (factCategory is null)
		{
			factCategory = new Category
			{
				CategoryName = category,
				CategoryTypeId = StaticValues.QuoteCategoryTypeId
			};
			context.Categories.Add(factCategory);
			await context.SaveChangesAsync();
		}
		context.FactCategories.Add(new FactCategory
		{
			FactId = fact.FactId,
			CategoryId = factCategory.CategoryId
		});
	}
	await context.SaveChangesAsync();
}

static async Task RemoveCategoriesAsync(FactContext context, Fact fact)
{
	List<Category> categories = fact.FactCategories.Select(x => x.Category).ToList();
	context.RemoveRange(fact.FactCategories);
	await context.SaveChangesAsync();
	foreach (Category category in categories)
		if ((await context.FactCategories.CountAsync(x => x.CategoryId == category.CategoryId)) == 0)
			context.Categories.Remove(category);
	await context.SaveChangesAsync();
}

app.Run();