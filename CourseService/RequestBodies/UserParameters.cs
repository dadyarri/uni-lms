using src.Models;


namespace src.RequestBodies;

/// <summary>
/// Request body to Auth group methods
/// </summary>
public class UserParameters {
  public required string FirstName { get; set; } = string.Empty;
  public required string LastName { get; set; } = string.Empty;
  public string? Patronymic { get; set; } = string.Empty;
  public Guid? GroupId { get; set; }
  public int? Subgroup { get; set; }
  public required Guid RoleId { get; set; }
  public required string Email { get; set; }
  
  public required IFormFile Avatar { get; set; }
}