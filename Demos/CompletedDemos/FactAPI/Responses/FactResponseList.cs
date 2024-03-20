namespace TaleLearnCode.Facts.Responses;

public class FactResponseList
{
	public int TotalCount { get; set; }
	public int PageSize { get; set; }
	public int PageCount { get; set; }
	public int PageNumber { get; set; }
	public List<FactResponse> Facts { get; set; } = [];
}