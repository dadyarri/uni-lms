namespace src.Models;

/// <summary>
/// Model, which representing code of registration
/// </summary>
public class RegisterCode : BaseModel {
  /// <summary>
  /// Code of registration
  /// </summary>
  public required string Code;

  /// <summary>
  /// Flag, which means is code was used by someone
  /// </summary>
  public required bool IsValid;

  /// <summary>
  /// User, who used the code 
  /// </summary>
  public User? UsedBy;

  /// <summary>
  /// The time, at which the code was used
  /// </summary>
  public required DateTime UsedAt;
}