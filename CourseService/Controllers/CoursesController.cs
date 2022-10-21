using Microsoft.AspNetCore.Mvc;

using src.Models;


namespace src.Controllers;

/// <summary>
/// Controller for managing courses
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase {
  /// <summary>
  /// Dummy method that returining course by its ID
  /// </summary>
  /// <param name="courseId">ID of course to find</param>
  /// <returns>Model of course</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult<Course>> GetById(Guid courseId) {
    return Ok(
      new Course {
        Name = "Some course",
        Owner = new User {
          Email = "example@mail.com",
          FirstName = "John",
          LastName = "Doe",
          Role = new Role {
            Name = "Tutor",
            Description = "",
          },
        },
        AssignedGroups = new List<Group>(),
      }
    );
  }
}