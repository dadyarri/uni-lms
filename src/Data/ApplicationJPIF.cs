using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;


namespace src.Data;

/// <summary>
/// PATCH request body formatter
/// </summary>
public class ApplicationJpif {
  /// <summary>
  /// Gets the PATCH request body formatter
  /// </summary>
  /// <returns></returns>
  public static NewtonsoftJsonInputFormatter GetJsonPatchInputFormatter() {
    var builder = new ServiceCollection()
                  .AddLogging()
                  .AddMvc()
                  .AddNewtonsoftJson()
                  .Services
                  .BuildServiceProvider();

    return builder
           .GetRequiredService<IOptions<MvcOptions>>()
           .Value
           .InputFormatters
           .OfType<NewtonsoftJsonInputFormatter>()
           .First();
  }
}