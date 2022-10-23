using Microsoft.EntityFrameworkCore;


namespace src.Responses;

/// <summary>
/// Paginated list
/// </summary>
/// <typeparam name="TModel">Type of items in paged</typeparam>
[Serializable]
public class Paged<TModel> {
  /// <summary>
  /// Items from selected page
  /// </summary>
  public List<TModel> Items { get; init; }

  /// <summary>
  /// Current page
  /// </summary>
  public int CurrentPage { get; init; }

  /// <summary>
  /// Total pages
  /// </summary>
  public int TotalPages { get; init; }

  /// <summary>
  /// Size of page
  /// </summary>
  public int PageSize { get; init; }

  /// <summary>
  /// Total elements
  /// </summary>
  public int TotalCount { get; init; }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="items">List of models</param>
  /// <param name="count">Amount of elements</param>
  /// <param name="pageNumber">Page number</param>
  /// <param name="pageSize">Page size</param>
  public Paged(List<TModel> items, int count, int pageNumber, int pageSize) {
    if (count == 0) {
      TotalPages = 0;
    } else {
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    TotalCount = count;
    PageSize = pageSize;
    CurrentPage = pageNumber;
    Items = items;
  }

  /// <summary>
  /// Whether there is previous page
  /// </summary>
  public bool HasPrevious => CurrentPage > 1;

  /// <summary>
  /// Whether there is next page
  /// </summary>
  public bool HasNext => CurrentPage < TotalPages;

  /// <summary>
  /// Converting query results to paginated result
  /// </summary>
  /// <param name="source">Query results</param>
  /// <param name="pageNumber">Number of page to fetch</param>
  /// <param name="pageSize">Size of page</param>
  /// <returns>Paginated result</returns>
  public static async Task<Paged<TModel>> ToPaged(
    IQueryable<TModel> source, int pageNumber, int pageSize
  ) {
    var count = source.Count();
    List<TModel> items;

    if (count == 0) {
      items = new List<TModel>();
    } else {
      items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    return new Paged<TModel>(
      items,
      count,
      pageNumber,
      pageSize
    );
  }
}