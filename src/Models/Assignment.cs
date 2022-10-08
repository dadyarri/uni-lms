namespace src.Models;

/// <summary>
/// Model, which representing a piece of work that user is given to do
/// </summary>
public class Assignment : BaseModel {
  /// <summary>
  /// Category of course
  /// </summary>
  public required object Category;

  /// <summary>
  /// Name of assignment
  /// </summary>
  public required string Name;

  /// <summary>
  /// Type of assignmnent
  /// </summary>
  public required AssignmentType Type;

  /// <summary>
  /// limit of amount of files
  /// </summary>
  public int? AmountOfFilesLimit;

  /// <summary>
  /// limit of size of file
  /// </summary>
  public int? SizeOfOneFileLimit;

  /// <summary>
  /// List of attachments
  /// </summary>
  public List<Attachment>? Attachments;

  /// <summary>
  /// List of deadlines
  /// </summary>
  public List<Deadline>? Deadlines;
}