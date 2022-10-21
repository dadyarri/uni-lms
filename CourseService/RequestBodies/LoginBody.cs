namespace src.RequestBodies; 

/// <summary>
/// Body for Login endpoint
/// </summary>
public class LoginBody {
  /// <summary>
  /// Email of user
  /// </summary>
  public required string Email { get; set; }
  /// <summary>
  /// Password of user
  /// </summary>
  public required string Password { get; set; }
  
}