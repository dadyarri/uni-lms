using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
  /// Controller constructor
  /// </summary>
  /// <param name="context">Automatically added database context</param>
  /// <param name="configuration">Automatically added project configuration</param>
  public AuthController(ApplicationContext context, IConfiguration configuration) {
    _db = context;
    _configuration = configuration;
  }

  [HttpPost("Create")]
  [Produces("application/json")]
  public async Task<ActionResult<User>> Create(UserParameters body) {
    var user = new User {
      FirstName = body.FirstName,
      LastName = body.LastName,
      Patronymic = body.Patronymic,
      Group = body.Group,
      Subgroup = body.Subgroup,
      Role = body.Role,
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

    var registerCode = await InsertNewRegisterCode(user);
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

  // /// <summary>
  // /// User registration
  // /// </summary>
  // /// <param name="body">Request body with username and password</param>
  // /// <response code="201">User created</response>
  // /// <response code="409">Conflict (there is already a user with the same name)</response>
  // /// <response code="500">Server error</response>
  // [AllowAnonymous]
  // [HttpPost("Register")]
  // [Produces("application/json")]
  // [ProducesResponseType(StatusCodes.Status201Created)]
  // [ProducesResponseType(StatusCodes.Status409Conflict)]
  // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  // public async Task<ActionResult<User>> Register(UserParameters body) { }

  // /// <summary>
  // /// Аутентификация пользователя и генерация JWT
  // /// </summary>
  // /// <param name="body">Логин и пароль пользователя</param>
  // /// <response code="200">Токен сгенерирован</response>
  // /// <response code="400">Невалидные данные (имя пользователя/пароль)</response>
  // [AllowAnonymous]
  // [HttpPost("Login")]
  // [ProducesResponseType(StatusCodes.Status200OK)]
  // [ProducesResponseType(StatusCodes.Status400BadRequest)]
  // public async Task<ActionResult<string>> Login(UserParameters body) { }
  //
  // /// <summary>
  // /// Получение информации о текущем пользователе
  // /// </summary>
  // /// <response code="200">Данные получены</response>
  // /// <response code="401">Пользователь не авторизован</response>
  // [Authorize]
  // [HttpGet("Whoami")]
  // [ProducesResponseType(StatusCodes.Status200OK)]
  // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  // public ActionResult<UserResponse> Whoami() {
  //   var user = GetCurrentUser();
  //
  //   if (user is not null) {
  //     return Ok(user);
  //   }
  //
  //   return Unauthorized(
  //     new Error {
  //       Code = (int) HttpStatusCode.Unauthorized,
  //       Message = "User is not authorized"
  //     }
  //   );
  // }
  //
  // private UserResponse? GetCurrentUser() {
  //   if (HttpContext.User.Identity is not ClaimsIdentity identity) return null;
  //   var userClaims = identity.Claims;
  //   return new UserResponse {
  //     Username = userClaims.First(c => c.Type == ClaimTypes.Name).Value
  //   };
  // }
  //
  // private string GenerateToken(User user) {
  //   var claims = new List<Claim> {
  //     new(ClaimTypes.Name, user.Username),
  //   };
  //
  //   var rolesOfUser = _db.Roles.Where(
  //     r => r.Users.Any(u => u.Id == user.Id)
  //   );
  //
  //   foreach (var role in rolesOfUser) {
  //     claims.Add(new Claim(ClaimTypes.Role, role.Name));
  //   }
  //
  //   var key = new SymmetricSecurityKey(
  //     System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Security:Token").Value)
  //   );
  //
  //   var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
  //
  //   var token = new JwtSecurityToken(
  //     claims: claims,
  //     expires: DateTime.Now.AddMinutes(10),
  //     signingCredentials: credentials
  //   );
  //
  //   var jwt = new JwtSecurityTokenHandler().WriteToken(token);
  //
  //   return jwt;
  // }
  private void CreatePasswordHash(
    string password, out byte[] passwordHash, out byte[] passwordSalt
  ) {
    using var hmac = new HMACSHA512();
    passwordSalt = hmac.Key;
    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
  }

  private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {
    using var hmac = new HMACSHA512(passwordSalt);
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    return computedHash.SequenceEqual(passwordHash);
  }

  private string GenerateRandomString(int length) {
    const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,-!";
    Random random = new();
    return new string(
      Enumerable.Repeat(alphabet, length).Select(s => s[random.Next(s.Length)]).ToArray()
    );
  }
  
  private async Task<string> InsertNewRegisterCode(User user) {
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
          await InsertNewRegisterCode(user);
          break;
      }
    }

    return registerCode.Code;
  }
}