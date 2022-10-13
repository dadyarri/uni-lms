namespace src.Models;

/// <summary>
/// Parent model from which are inherited all other models
/// </summary>
public class BaseModel {
  /// <summary>
  /// Unique identifier 
  /// </summary>
  public Guid Id { get; init; }
}