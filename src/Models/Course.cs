namespace src.Models;

/// <summary>
/// Model, which representing study course
/// </summary>
public class Course : BaseModel {
  /// <summary>
  /// name of course
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// Abbreviation of course
  /// </summary>
  public string? Abbreviation { get; set; }

  /// <summary>
  /// Semester of Course
  /// </summary>
  public int? Semester { get; set; }

  /// <summary>
  /// Owner of Course
  /// </summary>
  public required User Owner { get; set; }

  /// <summary>
  /// Groups that are assigned to the course
  /// </summary>
  public required List<Group>? AssignedGroups { get; set; }
}