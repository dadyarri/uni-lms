using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Serilog;

using Swashbuckle.AspNetCore.SwaggerGen;

using ILogger = Serilog.ILogger;


namespace src.OperationFilters;

/// <summary>
/// Filter, which applies default security policy to every endpoints without AllowAnonymousAttribute
/// </summary>
public class AuthResponsesOperationFilter : IOperationFilter {
  private readonly ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

  /// <summary>
  /// Applies the filter
  /// </summary>
  /// <param name="operation">OpenAPI's endpoint</param>
  /// <param name="context">Filter's context</param>
  public void Apply(OpenApiOperation operation, OperationFilterContext context) {
    if (context.MethodInfo.DeclaringType == null) {
      _logger.Debug("{DeclaringType} is null, skipping", context.MethodInfo.DeclaringType);
      return;
    }

    if (context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
      _logger.Debug(
        "{TypeName}.{MethodName} has AllowAnonymous, skipping",
        context.MethodInfo.DeclaringType.Name,
        context.MethodInfo.Name
      );
      return;
    }

    if (context.MethodInfo
               .DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
      _logger.Debug(
        "{Name} has AllowAnonymous, skipping",
        context.MethodInfo.DeclaringType.Name
      );
      return;
    }

    _logger.Debug(
      "{TypeName}.{MethodName} hasn't AllowAnonymous, adding security policy",
      context.MethodInfo.DeclaringType.Name,
      context.MethodInfo.Name
    );

    operation.Security = new List<OpenApiSecurityRequirement>() {
      new() {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer",
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
          },
          new List<string>()
        },
      },
    };
  }
}