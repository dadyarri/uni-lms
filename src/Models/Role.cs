namespace src.Models;

/// <summary>
/// Model, which representing role of user
/// </summary>
public class Role:BaseModel {
  /// <summary>
  /// Name of role of user
  /// </summary>
  public required string Name;

  /// <summary>
  /// Description of role of user 
  /// </summary>
  public required string Description;
}