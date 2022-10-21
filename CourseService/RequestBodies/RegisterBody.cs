namespace src.RequestBodies; 

/// <summary>
/// Body for Register endpoint
/// </summary>
public class RegisterBody {
  /// <summary>
  /// Unique one-time register code
  /// </summary>
  public required string RegisterCode { get; set; }
  
  /// <summary>
  /// Password of user
  /// </summary>
  public required string Password { get; set; }
}