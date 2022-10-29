using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Serilog.Extensions.Logging;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace src.OperationFilters;

/// <summary>
/// Filter, which applies default security policy to every endpoints without AllowAnonymousAttribute
/// </summary>
public class SecurityPolicyOperationFilter : IOperationFilter {
  private readonly Logger<SecurityPolicyOperationFilter> _logger = new (new SerilogLoggerFactory());

  /// <summary>
  /// Applies the filter
  /// </summary>
  /// <param name="operation">OpenAPI's endpoint</param>
  /// <param name="context">Filter's context</param>
  public void Apply(OpenApiOperation operation, OperationFilterContext context) {
    if (context.MethodInfo.DeclaringType == null) {
      _logger.LogDebug("{DeclaringType} is null, skipping", context.MethodInfo.DeclaringType);
      return;
    }

    if (context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
      _logger.LogDebug(
        "{TypeName}.{MethodName} has AllowAnonymous, skipping",
        context.MethodInfo.DeclaringType.Name,
        context.MethodInfo.Name
      );
      return;
    }

    if (context.MethodInfo
               .DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
      _logger.LogDebug(
        "{Name} has AllowAnonymous, skipping",
        context.MethodInfo.DeclaringType.Name
      );
      return;
    }

    _logger.LogDebug(
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