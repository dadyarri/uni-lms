using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace src.OperationFilters;

/// <summary>
/// Filter, which applies default security policy to every endpoints without AllowAnonymousAttribute
/// </summary>
public class AuthResponsesOperationFilter : IOperationFilter {
  /// <summary>
  /// Applies the filter
  /// </summary>
  /// <param name="operation">OpenAPI's endpoint</param>
  /// <param name="context">Filter's context</param>
  public void Apply(OpenApiOperation operation, OperationFilterContext context) {
    if (context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
      return;
    }

    if (context.MethodInfo.DeclaringType == null) {
      return;
    }

    if (context.MethodInfo
               .DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
      return;
    }

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