using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Npgsql;

using src.Data;
using src.Exceptions;
using src.Models;
using src.RequestBodies;
using src.Responses;


namespace src.Controllers;

/// <summary>
/// Working with users (registration/authorization/authentication...)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
  private readonly IConfiguration _configuration;
  private readonly ApplicationContext _db;

  /// <summary>
  /// Controller's constructor
  /// </summary>
  /// <param name="context">Automatically injected database context</param>
  /// <param name="configuration">Automatically injected project configuration</param>
  public AuthController(ApplicationContext context, IConfiguration configuration) {
    _db = context;
    _configuration = configuration;
  }

  /// <summary>
  /// Creating user by administrator (filling data, known by educational organization)
  /// </summary>
  /// <param name="body">User's personal data</param>
  /// <returns>Model of created users</returns>
  /// <exception cref="MissingConfigurationValueException">Raised, if configuration lacks of required properties</exception>
  [HttpPost("Create")]
  [Produces("application/json")]
  public async Task<ActionResult<User>> Create(UserParameters body) {

    var group = await _db.Groups.FindAsync(body.GroupId);
    var role = await _db.Roles.FindAsync(body.RoleId);

    if (group == null) {
      return NotFound(
        new Error {
          Code = (int)HttpStatusCode.NotFound,
          Message = "GroupId with this id doesn't exist",
          Data = body.GroupId,
        }
      );
    }

    if (role == null) {
      return NotFound(
        new Error {
          Code = (int)HttpStatusCode.NotFound,
          Message = "RoleId with this id doesn't exist",
          Data = body.RoleId,
        }
      );
    }
    
    var user = new User {
      FirstName = body.FirstName,
      LastName = body.LastName,
      Patronymic = body.Patronymic,
      Group = group,
      Subgroup = body.Subgroup,
      Role = role,
      Email = body.Email,
    };
    await _db.Users.AddAsync(user);
    try {
      await _db.SaveChangesAsync();
    }
    catch (DbUpdateException ex) when
      (ex.InnerException is PostgresException exception) {
      return exception.SqlState switch {
        PostgresErrorCodes.UniqueViolation => Conflict(
          new Error {
            Code = (int)HttpStatusCode.Conflict,
            Message = "User already exist",
            Data = body.Email,
          }
        ),
        _ => Problem(exception.MessageText),
      };
    }

    var emailAddress =
      _configuration.GetRequiredSection("MailSettings").GetValue<string>("Address");

    var emailToken = _configuration.GetRequiredSection("MailSettings").GetValue<string>("Token");

    var emailHost = _configuration.GetRequiredSection("MailSettings").GetValue<string>("Host");

    if (emailAddress == null) {
      throw new MissingConfigurationValueException(
        "MailSettings.Address configuration value is required"
      );
    }

    if (emailToken == null) {
      throw new MissingConfigurationValueException(
        "MailSettings.Token configuration value is required"
      );
    }

    if (emailHost == null) {
      throw new MissingConfigurationValueException(
        "MailSettings.Host configuration value is required"
      );
    }

    var registerCode = await CreateRegisterCode(user);
    var smtpClient = new SmtpClient(emailHost) {
      Port = 465,
      Credentials = new NetworkCredential(emailAddress, emailToken),
      EnableSsl = true,
    };
    var mailMessage = new MailMessage {
      From = new MailAddress(emailAddress),
      Subject = "Код регистрации для образовательной платформы",
      Body =
        $"<h1>Здравствуйте, {body.FirstName}!</h1> <p>Ваш адрес электронной почты был указан при создании аккаунта на сайте образовательной платформы. Зарегистрируйте аккаунт, перейдя по ссылке: https://localhost:8000/Register?code={registerCode}.</p><p>Если это письмо пришло вам по ошибке, проигнорируйте его.</p>",
      IsBodyHtml = true,
    };
    mailMessage.To.Add(body.Email);

    smtpClient.Send(mailMessage);
    return Created("/api/Auth/Create", user);
  }

  /// <summary>
  /// Authenticate user and generate JWT
  /// </summary>
  /// <param name="body">Login and password of user</param>
  /// <response code="200">Token was generated</response>
  /// <response code="400">Invalid credentials</response>
  [AllowAnonymous]
  [HttpPost("Login")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<string>> Login(LoginBody body) {
    var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.Equals(body.Email));

    if (user is null) {
      return BadRequest(
        new Error {
          Code = (int)HttpStatusCode.BadRequest,
          Message = "User was not found",
          Data = body.Email,
        }
      );
    }

    if (user.PasswordHash == null || user.PasswordSalt == null) {
      throw new PasswordWasNotSetException();
    }

    if (!VerifyPasswordHash(body.Password, user.PasswordHash, user.PasswordSalt)) {
      return BadRequest(
        new Error {
          Code = (int)HttpStatusCode.BadRequest,
          Message = "Wrong password",
        }
      );
    }

    return Ok(GenerateToken(user));
  }

  /// <summary>
  /// Fetching information about current user
  /// </summary>
  /// <response code="200">Data fetched</response>
  /// <response code="401">Not authorized</response>
  [Authorize]
  [HttpGet("Whoami")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public ActionResult<UserResponse> Whoami() {
    var user = GetCurrentUser();

    if (user is not null) {
      return Ok(user);
    }

    return Unauthorized(
      new Error {
        Code = (int)HttpStatusCode.Unauthorized,
        Message = "User is not authorized",
      }
    );
  }

  private UserResponse? GetCurrentUser() {
    if (HttpContext.User.Identity is not ClaimsIdentity identity) {
      return null;
    }

    var userClaims = identity.Claims;
    var email = userClaims.First(c => c.Type == ClaimTypes.Name).Value;
    var user = _db.Users.First(u => u.Email.Equals(email));

    return new UserResponse {
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
    };
  }

  private string GenerateToken(User user) {
    var claims = new List<Claim> {
      new(ClaimTypes.Name, user.Email),
    };
    var role = _db.Roles.First(r => r.Id == user.Role.Id);

    claims.Add(new Claim(ClaimTypes.Role, role.Name));

    string? securityToken;

    try {
      securityToken = _configuration.GetRequiredSection("Security").GetValue<string>("Token");
    }
    catch (InvalidOperationException) {
      throw new MissingConfigurationValueException(
        "Security.Token configuration value is required"
      );
    }

    if (securityToken == null) {
      throw new MissingConfigurationValueException(
        "Security.Token configuration value is required"
      );
    }

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(securityToken)
    );

    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.Now.AddMinutes(10),
      signingCredentials: credentials
    );

    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }

  private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {
    using var hmac = new HMACSHA512(passwordSalt);
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    return computedHash.SequenceEqual(passwordHash);
  }

  private static string GenerateRandomString(int length) {
    const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,-!";
    Random random = new();
    return new string(
      Enumerable.Repeat(alphabet, length).Select(s => s[random.Next(s.Length)]).ToArray()
    );
  }

  private async Task<string> CreateRegisterCode(User user) {
    var randomString = GenerateRandomString(5);
    var registerCode = new RegisterCode {
      Code = randomString,
      IsValid = true,
      UsedBy = user,
    };
    await _db.RegisterCodes.AddAsync(registerCode);
    try {
      await _db.SaveChangesAsync();
    }
    catch (DbUpdateException ex) when
      (ex.InnerException is PostgresException exception) {
      switch (exception.SqlState) {
        case PostgresErrorCodes.UniqueViolation:
          await CreateRegisterCode(user);
          break;
      }
    }

    return registerCode.Code;
  }
}