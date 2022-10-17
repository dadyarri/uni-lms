using System.Net;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using src.Data;
using src.Models;
using src.RequestBodies;
using src.Responses;


namespace src.Controllers;

/// <summary>
/// Controller to create/update/delete users
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
  private readonly ILogger<UserController> _logger;
  private readonly ApplicationContext _db;

  /// <summary>
  /// Controller constructor
  /// </summary>
  /// <param name="context">Automatically injected database context</param>
  /// <param name="logger">Automatically injected logger</param>
  public UserController(ApplicationContext context, ILogger<UserController> logger) {
    _db = context;
    _logger = logger;
  }

  /// <summary>
  /// Get user by id 
  /// </summary>
  /// <param name="id"></param>
  ///  <response code="200">OK</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="404">Not found</response>
  /// <response code="500">Internal server error</response>
  /// <returns>Role object</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<User>> GetUserById(Guid id) {
    {
      var user = await _db.Roles.FindAsync(id);

      if (user is null) {
        return NotFound(
          new Error {
            Code = (int) HttpStatusCode.NotFound,
            Message = "User with this id was not found",
            Data = id,
          }
        );
      }

      return Ok(User);
    }
  }

  /// <summary>
  /// Get all available users
  /// </summary>
  /// <param name="parameters">Params of pagination</param>
  /// <response code="200">OK</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="500">Internal server error</response>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<Paged<Role>>> GetAllUsers(
    [FromQuery] PagingParameters parameters
  ) {
    var queryResults = _db.Roles.OrderBy(p => p.Id);

    Paged<Role> roles =
      await Paged<Role>.ToPaged(queryResults, parameters.PageNumber, parameters.PageSize);
    return Ok(roles);
  }

  /// <summary>
  /// Create user
  /// </summary>
  /// <param name="user">Model of user</param>
  /// <response code="201">Role is created</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="500">Internal server error</response>
  [HttpPost]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<User>> CreateUser(User user) {
    var created = _db.Users.Add(user);
    await _db.SaveChangesAsync();
    return Created("/api/Controller", created.Entity);
  }

  /// <summary>
  /// Delete user
  /// </summary>
  /// <param name="id">Id of user</param>
  /// <response code="204">User is deleted</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="404">Not found</response>
  /// <response code="500">Internal server error</response>
  [HttpDelete("{id:Guid}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult> DeleteUser(Guid id) {
    var entity = await _db.Users.FindAsync(id);

    if (entity is null) {
      return NotFound(
        new Error {
          Code = (int) HttpStatusCode.NotFound,
          Message = "User with this id was not found",
          Data = id,
        }
      );
    }

    _db.Users.Remove(entity);
    await _db.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Update user
  /// </summary>
  /// <param name="id">Id of user</param>
  /// <param name="role">JSON PATCH request updating a resource</param>
  /// <response code="200">OK</response>
  /// <response code="400">Bad request</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="404">Not found</response>
  /// <response code="500">Internal server error</response>
  [HttpPatch("{id:Guid}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<User>> UpdateUser(
    Guid id, [FromBody] JsonPatchDocument<User>? user
  ) {
    if (user == null) {
      return BadRequest(
        new Error {
          Code = (int) HttpStatusCode.BadRequest,
          Message = "User was not passed",
        }
      );
    }

    var entity = await _db.Users.FindAsync(id);

    if (entity == null) {
      return NotFound(
        new Error {
          Code = (int) HttpStatusCode.NotFound,
          Message = "User with this id was not found",
          Data = id,
        }
      );
    }

    user.ApplyTo(entity, ModelState);
    await _db.SaveChangesAsync();
    return Ok(entity);
  }
}