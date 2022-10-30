using src.Models;


namespace src.Responses;

/// <summary>
/// Response for /api/Auth/Preregister
/// </summary>
public sealed class PreregisterResponse {
  /// <summary>
  /// First name of newly created user
  /// </summary>
  public required string FirstName { get; init; }

  /// <summary>
  /// Last name of newly created user
  /// </summary>
  public required string LastName { get; init; }

  /// <summary>
  /// Patronymic of newly created user
  /// </summary>
  public string? Patronymic { get; init; }

  /// <summary>
  ///  Group of newly created user
  /// </summary>
  public Group? Group { get; set; }

  /// <summary>
  /// Subgroup of newly created user
  /// </summary>
  public int? Subgroup { get; set; }

  /// <summary>
  /// Name of role of newly created user
  /// </summary>
  public required string RoleName { get; set; }

  /// <summary>
  /// Profile picture of newly created user
  /// (ID in the File microservice)
  /// </summary>
  public Guid? Avatar { get; set; }

  /// <summary>
  /// Email of  newly created user
  /// </summary>
  public required string Email { get; set; }

  /// <summary>
  ///  Register code for newly created user
  /// </summary>
  public required string RegisterCode { get; set; }
}