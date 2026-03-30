using Microsoft.AspNetCore.ResponseCompression;
using Trivia_Assignment_backend.Managers;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<QuestionSessionManager>();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
});
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 65536;
    options.SizeLimit = 65536;
    options.UseCaseSensitivePaths = false;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000/")
                                .WithMethods("GET", "POST");
                      });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseResponseCaching();
app.UseResponseCompression();
app.UseCors(MyAllowSpecificOrigins);

app.Run();
