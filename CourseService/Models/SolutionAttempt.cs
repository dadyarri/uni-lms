namespace src.Models;

/// <summary>
/// Model, which representing attempt of solution of assignment
/// </summary>
public class SolutionAttempt : BaseModel {
  /// <summary>
  /// A piece of work that student is given to do
  /// </summary>
  public required Assignment Assignment { get; set; }

  /// <summary>
  /// Student, who submitted the attempt
  /// </summary>
  public required User Author { get; set; }

  /// <summary>
  /// Timestamp of submiting of the attempt
  /// </summary>
  public required DateTime SubmittedAt { get; set; }
}