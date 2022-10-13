namespace src.Models;

/// <summary>
/// Model, which representing type of assignment
/// </summary>
public class AssignmentType : BaseModel {
  /// <summary>
  /// Name of assignment type
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// Icon of assignment type
  /// </summary>
  public required Attachment Icon { get; set; }
}