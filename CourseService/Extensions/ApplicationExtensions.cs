namespace src.Extensions; 

/// <summary>
/// Extensions for <see cref="WebApplication"/>
/// </summary>
public static class ApplicationExtensions {
  /// <summary>
  /// Configure Swagger
  /// </summary>
  /// <remarks>
  /// 1. Enable Swagger in every environment<br/>
  /// 2. Set URL for OpenAPI JSON file
  /// </remarks>
  /// <param name="application"><see cref="WebApplication"/>'s object</param>
  public static void ConfigureSwagger(this WebApplication application) {
    application.UseSwagger();
    application.UseSwaggerUI(
      options => {
        options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
      }
    );
  }
}