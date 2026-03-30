using Microsoft.AspNetCore.ResponseCompression;
using Trivia_Assignment_backend.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
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
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<QuestionSessionManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseResponseCaching();
app.UseResponseCompression();

app.Run();
