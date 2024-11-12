using FitSync.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Dependencies.MigrateDatabase(app.Services);
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.Run();

public partial class Program { }

