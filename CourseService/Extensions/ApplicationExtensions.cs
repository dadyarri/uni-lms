namespace src.Extensions; 

/// <summary>
/// 
/// </summary>
public static class ApplicationExtensions {
  /// <summary>
  /// 
  /// </summary>
  /// <param name="application"></param>
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