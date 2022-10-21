using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using src.Data;
using src.Extensions;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

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
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
  }
);

builder.Configuration.ValidateConfiguration();

var connectionString = builder.Configuration.GetConnectionString(Environment.UserName);
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
var app = builder.Build();


if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(
    options => {
      options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
      options.RoutePrefix = string.Empty;
    }
  );
}

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  var context = services.GetRequiredService<ApplicationContext>();
  if (context.Database.GetPendingMigrations().Any())
  {
    context.Database.Migrate();
  }
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();