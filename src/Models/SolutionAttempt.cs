namespace src.Models;

/// <summary>
/// Model, which representing attempt of solution of assignment
/// </summary>
public class SolutionAttempt : BaseModel {
  /// <summary>
  /// A piece of work that user is given to do
  /// </summary>
  public required Assignment Assignment;

  /// <summary>
  /// The author of attempt
  /// </summary>
  public required User Author;

  /// <summary>
  /// Timestamp of submiting
  /// </summary>
  public required DateTime SubmittedAt;
}