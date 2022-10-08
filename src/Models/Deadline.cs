namespace src.Models;

/// <summary>
/// Model, which representing time by which assignment should be done
/// </summary>
public class Deadline {
  /// <summary>
  /// Piece of work that user is given to do
  /// </summary>
  public required Assignment Assignment;

  /// <summary>
  /// Group of student
  /// </summary>
  public required Group Group;

  /// <summary>
  /// Piece of group
  /// </summary>
  public int? Subgroup;

  /// <summary>
  /// Time by which assignmet schould be submitted
  /// </summary>
  public required DateTime SubmitBy;
}