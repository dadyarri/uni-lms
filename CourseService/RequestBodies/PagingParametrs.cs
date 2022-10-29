namespace src.RequestBodies;

/// <summary>
/// Pagination parameters
/// </summary>
public class PagingParameters {
  /// <summary>
  /// Number of page
  /// </summary>
  public int? PageNumber { get; init; }

  /// <summary>
  /// Number of items per page
  /// </summary>
  public int? PageSize { get; init; }
}