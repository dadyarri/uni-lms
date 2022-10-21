namespace src.Responses; 

/// <summary>
/// Response for WhoAmI endpoint
/// </summary>
public class UserResponse {
  /// <summary>
  /// First name of user
  /// </summary>
  public required string FirstName { get; set; }
  /// <summary>
  /// Last name of user
  /// </summary>
  public required string LastName { get; set; }
  /// <summary>
  /// Email of user
  /// </summary>
  public required string Email { get; set; }
}