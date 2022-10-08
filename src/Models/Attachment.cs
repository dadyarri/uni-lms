namespace src.Models;

/// <summary>
/// Model, which representing aploaded attachment
/// </summary>
public class Attachment : BaseModel {
  /// <summary>
  /// MD5 Checksum of file
  /// </summary>
  public required string Checksum;

  /// <summary>
  /// Name of file which shows in UI
  /// </summary>
  public required string VisibleName;

  /// <summary>
  /// Link to file on static file hosting
  /// </summary>
  public required string FileUrl;
}