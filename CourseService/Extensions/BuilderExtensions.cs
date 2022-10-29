using System.Diagnostics;

using Serilog;
using Serilog.Events;


namespace src.Extensions; 

/// <summary>
/// Extensions for <see cref="WebApplicationBuilder"/>
/// </summary>
public static class BuilderExtensions {
  /// <summary>
  /// Enable Serilog as default logger system
  /// </summary>
  /// <param name="builder"><see cref="WebApplicationBuilder"/>'s object</param>
  public static void ConfigureSerilog(this WebApplicationBuilder builder) {
    builder.Host.UseSerilog();
    builder.Services.AddLogging(options => options.AddSerilog(dispose: true));
    
    var loggerConfiguration = new LoggerConfiguration()
                              .Enrich.FromLogContext().WriteTo.Console();

    if (builder.Environment.EnvironmentName == "Development" || Debugger.IsAttached) {
      loggerConfiguration.MinimumLevel.Debug()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Debug);
    } else {
      loggerConfiguration.MinimumLevel.Information()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
    }

    Log.Logger = loggerConfiguration.CreateLogger();
  }
}