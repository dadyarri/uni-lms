using src.Models;


namespace src.Dtos; 

/// <summary>
/// Model, which representing aploaded attachment
/// </summary>
public class AttachmentDto : BaseModel {
  /// <summary>
  /// MD5 Checksum of file
  /// </summary>
  public required string Checksum { get; set; }

  /// <summary>
  /// Name of file which shows in UI
  /// </summary>
  public required string VisibleName { get; set; }

  /// <summary>
  /// Name of file
  /// </summary>
  public required string FileName { get; set; }
}