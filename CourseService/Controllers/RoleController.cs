  using System.Net;

  using Microsoft.AspNetCore.JsonPatch;
  using Microsoft.AspNetCore.Mvc;

  using src.Data;
  using src.Models;
  using src.RequestBodies;
  using src.Responses;


  namespace src.Controllers;

  /// <summary>
  /// Controller to create/update/delete roles
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class RoleController : ControllerBase {
    private readonly ILogger<RoleController> _logger;
    private readonly ApplicationContext _db;

    /// <summary>
    /// Controller constructor
    /// </summary>
    /// <param name="context">Automatically injected database context</param>
    /// <param name="logger">Automatically injected logger</param>
    public RoleController(ApplicationContext context, ILogger<RoleController> logger) {
      _db = context;
      _logger = logger;
    }

    /// <summary>
    /// Get role by id 
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
    public async Task<ActionResult<Role>> GetRoleById(Guid id) {
      {
        var role = await _db.Roles.FindAsync(id);

        if (role is null) {
          return NotFound(
            new Error {
              Code = (int) HttpStatusCode.NotFound,
              Message = "Role with this id was not found",
              Data = id,
            }
          );
        }

        return Ok(role);
      }
    }

    /// <summary>
    /// Get all available roles
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
    public async Task<ActionResult<Paged<Role>>> GetAllRoles(
      [FromQuery] PagingParameters parameters
    ) {
      var queryResults = _db.Roles.OrderBy(p => p.Id);

      Paged<Role> roles =
        await Paged<Role>.ToPaged(queryResults, parameters.PageNumber, parameters.PageSize);
      return Ok(roles);
    }

    /// <summary>
    /// Create role
    /// </summary>
    /// <param name="role">Model of role</param>
    /// <response code="201">Role is created</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Role>> CreateRole(Role role) {
      var created = _db.Roles.Add(role);
      await _db.SaveChangesAsync();
      return Created("/api/Role", created.Entity);
    }

    /// <summary>
    /// Delete role
    /// </summary>
    /// <param name="id">Id of role</param>
    /// <response code="204">Role is deleted</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{id:Guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteRole(Guid id) {
      var entity = await _db.Roles.FindAsync(id);
      if (entity is null) {
        return NotFound(
          new Error {
            Code = (int) HttpStatusCode.NotFound,
            Message = "Role with this id was not found",
            Data = id,
          }
        );
      }

      _db.Roles.Remove(entity);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="id">Id of role</param>
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
    public async Task<ActionResult<Role>> UpdateRole(
      Guid id, [FromBody] JsonPatchDocument<Role>? role
    ) {
      if (role == null) {
        return BadRequest(
          new Error {
            Code = (int) HttpStatusCode.BadRequest,
            Message = "Role was not passed",
          }
        );
      }

      var entity = await _db.Roles.FindAsync(id);

      if (entity == null) {
        return NotFound(
          new Error {
            Code = (int) HttpStatusCode.NotFound,
            Message = "Role with this id was not found",
            Data = id,
          }
        );
      }

      role.ApplyTo(entity, ModelState);
      await _db.SaveChangesAsync();
      return Ok(entity);
    }
  }