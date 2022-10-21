using Microsoft.EntityFrameworkCore;


namespace src.Responses;

/// <summary>
/// Paginated list
/// </summary>
/// <typeparam name="TModel">Type of items in paged</typeparam>
public class Paged<TModel> {
  /// <summary>
  /// Items from selected page
  /// </summary>
  public readonly List<TModel> Items;

  /// <summary>
  /// Current page
  /// </summary>
  public readonly int CurrentPage;

  /// <summary>
  /// Total pages
  /// </summary>
  public readonly int TotalPages;

  /// <summary>
  /// Size of page
  /// </summary>
  public int PageSize;

  /// <summary>
  /// Total elements
  /// </summary>
  public int TotalCount;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="items">List of models</param>
  /// <param name="count">Amount of elements</param>
  /// <param name="pageNumber">Page number</param>
  /// <param name="pageSize">Page size</param>
  public Paged(List<TModel> items, int count, int pageNumber, int pageSize) {
    TotalCount = count;
    PageSize = pageSize;
    CurrentPage = pageNumber;
    TotalPages = (int) Math.Ceiling(count / (double) pageSize);
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
    int count = source.Count();
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    return new Paged<TModel>(
      items,
      count,
      pageNumber,
      pageSize
    );
  }
}