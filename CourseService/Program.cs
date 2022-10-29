using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Serilog;

using src.Data;
using src.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog();

builder.Services.RegisterControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
  options => {
    options.TokenValidationParameters = new TokenValidationParameters {
      RequireAudience = false,
      ValidateAudience = false,
      ValidateIssuer = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Security:Token"])),
    };
  }
);

builder.Configuration.ValidateConfiguration();

var connectionString = builder.Configuration.GetConnectionString(Environment.UserName);
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();
app.ConfigureSwagger();

using (var scope = app.Services.CreateScope()) {
  var services = scope.ServiceProvider;

  var context = services.GetRequiredService<ApplicationContext>();
  if (context.Database.GetPendingMigrations().Any()) {
    context.Database.Migrate();
  }
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();