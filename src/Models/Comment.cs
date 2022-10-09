namespace src.Models;

/// <summary>
/// Model, which representing commet on solution attempt
/// </summary>
public class Comment : BaseModel {
  /// <summary>
  /// Text of comment
  /// </summary>
  public required string Text;

  /// <summary>
  /// Solution attempt to which a comment is written
  /// </summary>
  public required SolutionAttempt SolutionAttempt;

  /// <summary>
  /// Author of comment
  /// </summary>
  public required User Author;

  /// <summary>
  /// Parent comment of a comment
  /// </summary>
  public Comment? ParentComment;
}