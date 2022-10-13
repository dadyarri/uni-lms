namespace src.Models;

/// <summary>
/// Model, which representing time by which assignment should be done
/// </summary>
public class Deadline : BaseModel {
  /// <summary>
  /// Piece of work that user is given to do
  /// </summary>
  public required Assignment Assignment { get; set; }

  /// <summary>
  /// Group of student
  /// </summary>
  public required Group Group { get; set; }

  /// <summary>
  /// Piece of group
  /// </summary>
  public int? Subgroup { get; set; }

  /// <summary>
  /// Time by which assignmet schould be submitted
  /// </summary>
  public required DateTime SubmitBy { get; set; }
}