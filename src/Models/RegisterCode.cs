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
  /// Is code valid
  /// </summary>
  public required bool IsValid;

  /// <summary>
  /// User who use code 
  /// </summary>
  public User? UsedBy;

  /// <summary>
  /// Code registration time
  /// </summary>
  public required DateTime UsedAt;
}