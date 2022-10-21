using src.Models;


namespace src.RequestBodies;

/// <summary>
/// Request body to Auth group methods
/// </summary>
public class UserParameters {
  public required string FirstName { get; set; } = string.Empty;
  public required string LastName { get; set; } = string.Empty;
  public string? Patronymic { get; set; } = string.Empty;
  public Group? Group { get; set; }
  public int? Subgroup { get; set; }
  public required Role Role { get; set; }
  public required string Email { get; set; }
}