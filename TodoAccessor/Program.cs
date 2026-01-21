using Microsoft.EntityFrameworkCore;
using TodoAccessor.Data;
using TodoAccessor.Endpoints;
using TodoAccessor.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext when we add PostgreSQL
builder.Services.AddDbContext<TodoDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("TodoDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Temporarily disabled for easier debugging
// app.UseHttpsRedirection();

//app.UseAuthorization();

// Apply database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}

// Map all todo endpoints using extension method
app.MapTodoEndpoints();

app.Run();