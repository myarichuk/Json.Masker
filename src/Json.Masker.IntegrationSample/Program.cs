using Json.Masker.IntegrationSample.Services;
using Json.Masker.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddJsonMasking();
builder.Services.AddSingleton<FakeBusinessDataService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();