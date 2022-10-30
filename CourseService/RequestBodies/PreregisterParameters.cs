namespace src.RequestBodies;

/// <summary>
/// Request body to Register endpoint
/// </summary>
public class PreregisterParameters {
  /// <summary>
  /// First name of user
  /// </summary>
  public required string FirstName { get; set; } = string.Empty;

  /// <summary>
  /// Last name of user
  /// </summary>
  public required string LastName { get; set; } = string.Empty;

  /// <summary>
  /// Patronymic of user
  /// </summary>
  public string? Patronymic { get; set; } = string.Empty;

  /// <summary>
  /// Group id of user
  /// </summary>
  public Guid? GroupId { get; set; }

  /// <summary>
  /// Subgroup of user
  /// </summary>
  public int? Subgroup { get; set; }

  /// <summary>
  /// Role id of user
  /// </summary>
  public required Guid RoleId { get; set; }

  /// <summary>
  /// Email of user
  /// </summary>
  public required string Email { get; set; }

  /// <summary>
  /// Avatar of user
  /// </summary>
  public required IFormFile? Avatar { get; set; }
}