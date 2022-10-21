namespace src.RequestBodies;

/// <summary>
/// Pagination parameters
/// </summary>
public class PagingParameters {
  private const int MaxPageSize = 7;

  private readonly int _pageSize;

  /// <summary>
  /// Number of page
  /// </summary>
  public int PageNumber { get; init; } = 1;

  /// <summary>
  /// Number of items per page
  /// </summary>
  public int PageSize {
    get => _pageSize;
    init => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
  }
}