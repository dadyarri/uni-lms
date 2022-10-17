namespace src.Models;

/// <summary>
/// Model, which representing code of registration
/// </summary>
public class RegisterCode : BaseModel {
  /// <summary>
  /// Code of registration
  /// </summary>
  public required string Code { get; set; }

  /// <summary>
  /// Flag, which means is code was used by someone
  /// </summary>
  public required bool IsValid { get; set; }

  /// <summary>
  /// User, who used the code 
  /// </summary>
  public User? UsedBy { get; set; }

  /// <summary>
  /// The time, at which the code was used
  /// </summary>
  public DateTime? UsedAt { get; set; }
}