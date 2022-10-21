namespace src.Models;

/// <summary>
/// Model, which representing commet on solution attempt
/// </summary>
public class Comment : BaseModel {
  /// <summary>
  /// Text of comment
  /// </summary>
  public required string Text { get; set; }

  /// <summary>
  /// Solution attempt to which a comment is written
  /// </summary>
  public required SolutionAttempt SolutionAttempt { get; set; }

  /// <summary>
  /// Author of comment
  /// </summary>
  public required User Author { get; set; }

  /// <summary>
  /// Parent comment of a comment
  /// </summary>
  public Comment? ParentComment { get; set; }
}