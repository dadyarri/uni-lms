namespace src.Models;

/// <summary>
/// Model, which representing study course
/// </summary>
public class Course : BaseModel {
  /// <summary>
  /// name of course
  /// </summary>
  public required string Name;

  /// <summary>
  /// Abbreviation of course
  /// </summary>
  public string? Abbreviation;

  /// <summary>
  /// Semester of Course
  /// </summary>
  public int? Semester;

  /// <summary>
  /// Owner of Course
  /// </summary>
  public required User Owner;

  /// <summary>
  /// Groups that are assigned to the course
  /// </summary>
  public required List<Group>? AssignedGroups;
}