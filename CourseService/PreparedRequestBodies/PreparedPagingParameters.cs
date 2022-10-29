using src.RequestBodies;


namespace src.PreparedRequestBodies; 

public class PreparedPagingParameters {
  /// <summary>
  /// Number of page
  /// </summary>
  public int PageNumber { get; init; }

  /// <summary>
  /// Number of items per page
  /// </summary>
  public int PageSize { get; init; }

  /// <summary>
  /// Constructor of paging parameters
  /// </summary>
  /// <param name="parameters"></param>
  public PreparedPagingParameters(PagingParameters parameters) {
    PageNumber = parameters.PageNumber ?? 1;
    PageSize = parameters.PageSize ?? 10;
  }
}