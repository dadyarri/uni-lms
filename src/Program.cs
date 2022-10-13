using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
  options => {
    options.SwaggerDoc(
      "v1",
      new OpenApiInfo {
        Version = "v0.0.1",
        Title = "File API",
        Description =
          "API for upload and work with attachments"
      }
    );
  }
);


var app = builder.Build();


if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
  });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();