using Newtonsoft.Json;


namespace src.Models;

/// <summary>
/// Model, which representing user of a system
/// </summary>
public class User : BaseModel {
  /// <summary>
  /// First name of user
  /// </summary>
  public required string FirstName;

  /// <summary>
  /// Lasr name of user
  /// </summary>
  public required string LastName;

  /// <summary>
  /// Patronymic of user
  /// </summary>
  public string? Patronymic;

  /// <summary>
  ///  Group of user
  /// </summary>
  public Group? Group;

  /// <summary>
  /// Subgroup of user
  /// </summary>
  public int? Subgroup;

  /// <summary>
  /// Role of user
  /// </summary>
  public required Role Role;

  /// <summary>
  /// Profile picture of user
  /// </summary>
  public object? Avatar;

  /// <summary>
  /// Email of user
  /// </summary>
  public required string Email;

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