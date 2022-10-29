using System.Diagnostics;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Events;

using src.Data;
using src.Extensions;
using src.OperationFilters;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var loggerConfiguration = new LoggerConfiguration()
                          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                          .Enrich.FromLogContext().WriteTo.Console();

if (builder.Environment.EnvironmentName == "Development" || Debugger.IsAttached) {
  loggerConfiguration.MinimumLevel.Debug();
}

Log.Logger = loggerConfiguration.CreateLogger();


builder.Services.AddControllers(
  options => {
    options.InputFormatters.Insert(0, ApplicationJpif.GetJsonPatchInputFormatter());
  }
).AddNewtonsoftJson();

builder.Services.AddLogging(options => options.AddSerilog(dispose: true));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
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

    options.OperationFilter<AuthResponsesOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(
      Path.Combine(AppContext.BaseDirectory, xmlFilename),
      includeControllerXmlComments: true
    );
  }
);

builder.Services.AddSingleton<HttpClient>();

builder.Configuration.ValidateConfiguration();

var connectionString = builder.Configuration.GetConnectionString(Environment.UserName);
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(
  options => {
    options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
  }
);

using (var scope = app.Services.CreateScope()) {
  var services = scope.ServiceProvider;

  var context = services.GetRequiredService<ApplicationContext>();
  if (context.Database.GetPendingMigrations().Any()) {
    context.Database.Migrate();
  }
}

app.UseSerilogRequestLogging();
app.UseAuthorization();

app.MapControllers();

app.Run();