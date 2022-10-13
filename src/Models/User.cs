using Newtonsoft.Json;


namespace src.Models;

/// <summary>
/// Model, which representing user of a system
/// </summary>
public class User : BaseModel {
  /// <summary>
  /// First name of user
  /// </summary>
  public required string FirstName { get; set; }

  /// <summary>
  /// Lasr name of user
  /// </summary>
  public required string LastName { get; set; }

  /// <summary>
  /// Patronymic of user
  /// </summary>
  public string? Patronymic { get; set; }

  /// <summary>
  ///  Group of user
  /// </summary>
  public Group? Group { get; set; }

  /// <summary>
  /// Subgroup of user
  /// </summary>
  public int? Subgroup { get; set; }

  /// <summary>
  /// Role of user
  /// </summary>
  public required Role Role { get; set; }

  /// <summary>
  /// Profile picture of user
  /// </summary>
  public Attachment? Avatar { get; set; }

  /// <summary>
  /// Email of user
  /// </summary>
  public required string Email { get; set; }

  /// <summary>
  /// Hash of password of user
  /// </summary>
  [JsonIgnore]
  public required string PasswordHash;

  /// <summary>
  /// Salt of password of user
  /// </summary>
  [JsonIgnore]
  public required string PasswordSalt;
}