using System.Reflection;

using Microsoft.OpenApi.Models;

using src.Data;
using src.OperationFilters;


namespace src.Extensions; 

/// <summary>
/// 
/// </summary>
public static class ServicesExtensions {

  public static void RegisterControllers(this IServiceCollection services) {
    services.AddControllers(
      options => {
        options.InputFormatters.Insert(0, ApplicationJpif.GetJsonPatchInputFormatter());
      }
    ).AddNewtonsoftJson();
  }
  
  /// <summary>
  /// 
  /// </summary>
  /// <param name="services"></param>
  public static void ConfigureSwagger(this IServiceCollection services) {
    services.AddSwaggerGen(
      options => {
        options.SwaggerDoc(
          "v1",
          new OpenApiInfo {
            Version = "v0.0.1",
            Title = "Courses Service API",
            Description =
              "API for working with courses, users, groups, auth",
          }
        );

        options.AddSecurityDefinition(
          "Bearer",
          new OpenApiSecurityScheme {
            Description =
              "The project uses authentication with JWT. Input 'Bearer' [space] and your token after in the field below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
          }
        );

        options.OperationFilter<SecurityPolicyOperationFilter>();

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(
          Path.Combine(AppContext.BaseDirectory, xmlFilename),
          includeControllerXmlComments: true
        );
      }
    );
  }
}