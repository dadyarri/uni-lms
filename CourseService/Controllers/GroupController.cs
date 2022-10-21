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
/// Controller to create/update/delete groups
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase {
  private readonly ILogger<GroupController> _logger;
  private readonly ApplicationContext _db;

  /// <summary>
  /// Controller constructor
  /// </summary>
  /// <param name="context">Automatically injected database context</param>
  /// <param name="logger">Automatically injected logger</param>
  public GroupController(ApplicationContext context, ILogger<GroupController> logger) {
    _db = context;
    _logger = logger;
  }

  /// <summary>
  /// Get group by id 
  /// </summary>
  /// <param name="id"></param>
  ///  <response code="200">OK</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="404">Not found</response>
  /// <response code="500">Internal server error</response>
  /// <returns>Role object</returns>
  [HttpGet("{id:Guid}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<Group>> GetGroupById(Guid id) {
    {
      var group = await _db.Groups.FindAsync(id);

      if (group is null) {
        return NotFound(
          new Error {
            Code = (int)HttpStatusCode.NotFound,
            Message = "Group with this id was not found",
            Data = id,
          }
        );
      }

      return Ok(group);
    }
  }

  /// <summary>
  /// Get all available groups
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
  public async Task<ActionResult<Paged<Role>>> GetAllGroups(
    [FromQuery] PagingParameters parameters
  ) {
    var queryResults = _db.Groups.OrderBy(p => p.Name);

    Paged<Group> groups =
      await Paged<Group>.ToPaged(queryResults, parameters.PageNumber, parameters.PageSize);
    return Ok(groups);
  }

  /// <summary>
  /// Create group
  /// </summary>
  /// <param name="group">Model of group</param>
  /// <response code="201">Group is created</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="500">Internal server error</response>
  [HttpPost]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<Role>> CreateGroup(Group group) {
    var created = _db.Groups.Add(group);
    await _db.SaveChangesAsync();
    return Created("/api/Controller", created.Entity);
  }

  /// <summary>
  /// Delete group
  /// </summary>
  /// <param name="id">Id of group</param>
  /// <response code="204">Group is deleted</response>
  /// <response code="401">Unauthorized</response>
  /// <response code="404">Not found</response>
  /// <response code="500">Internal server error</response>
  [HttpDelete("{id:Guid}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult> DeleteGroup(Guid id) {
    var entity = await _db.Groups.FindAsync(id);

    if (entity is null) {
      return NotFound(
        new Error {
          Code = (int)HttpStatusCode.NotFound,
          Message = "Group with this id was not found",
          Data = id,
        }
      );
    }

    _db.Groups.Remove(entity);
    await _db.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Update group
  /// </summary>
  /// <param name="id">Id of group</param>
  /// <param name="group">JSON PATCH request updating a resource</param>
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
  public async Task<ActionResult<Group>> UpdateGroup(
    Guid id, [FromBody] JsonPatchDocument<Group>? group
  ) {
    if (group == null) {
      return BadRequest(
        new Error {
          Code = (int)HttpStatusCode.BadRequest,
          Message = "Group was not passed",
        }
      );
    }

    var entity = await _db.Groups.FindAsync(id);

    if (entity == null) {
      return NotFound(
        new Error {
          Code = (int)HttpStatusCode.NotFound,
          Message = "Group with this id was not found",
          Data = id,
        }
      );
    }

    group.ApplyTo(entity, ModelState);
    await _db.SaveChangesAsync();
    return Ok(entity);
  }
}