namespace src.Responses;

/// <summary>
/// The body of the response returned when an error occurs
/// </summary>
public class Error {
  /// <summary>
  /// HTTP error code
  /// </summary>
  public required int Code { get; set; }

  /// <summary>
  /// Error message
  /// </summary>
  public required string Message { get; set; }

  /// <summary>
  /// Optional data describing the cause of the error
  /// </summary>
  public object? Data { get; set; }
}