namespace src.Models;

/// <summary>
/// Model, which representing a piece of work that user is given to do
/// </summary>
public class Assignment : BaseModel {
  /// <summary>
  /// Category of course
  /// </summary>
  public required object Category { get; set; }

  /// <summary>
  /// Name of assignment
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// Type of assignmnent
  /// </summary>
  public required AssignmentType Type { get; set; }

  /// <summary>
  /// limit of amount of files
  /// </summary>
  public int? AmountOfFilesLimit { get; set; }

  /// <summary>
  /// limit of size of file
  /// </summary>
  public int? SizeOfOneFileLimit { get; set; }

  /// <summary>
  /// List of attachments
  /// </summary>
  public List<Attachment>? Attachments { get; set; }

  /// <summary>
  /// List of deadlines
  /// </summary>
  public List<Deadline>? Deadlines { get; set; }
}