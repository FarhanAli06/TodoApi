using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
var builder = WebApplication.CreateBuilder(args);
using var cnn = new SqliteConnection("Filename=:memory:");
cnn.Open();
builder.Services.AddDbContext<TodoContext>(o=>o.UseSqlite(cnn));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

//builder.WebHost.UseUrls("http://localhost:8082;https://localhost:8081");

var app = builder.Build();
var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<TodoContext>();
DbInitializer.Initialize(db);

//DbInitializer.Initialize(db);

//Health Check
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "GetTodoItem",
        pattern: "api/todoitems/{id}",
        defaults: new { controller = "TodoItems", action = "GetTodoItemAsync" }
    );
});



try
{
    app.Run();
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}

public partial class Program
{

}
