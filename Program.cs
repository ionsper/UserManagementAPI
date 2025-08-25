// UserManagementAPI - Minimal ASP.NET Core Web API for user CRUD, authentication, logging, and Swagger documentation.
// Assignment for the course: Back-End Development with .NET on Coursera.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using Serilog;
using Microsoft.OpenApi.Models;
using UserManagementAPI;
using UserManagementAPI.Models;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagementAPI", Version = "v1" });
});

builder.Host.UseSerilog();

var app = builder.Build();

app.Urls.Add("http://localhost:5000");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagementAPI v1");
    options.RoutePrefix = "swagger";
});

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"Internal server error.\"}");
    }
});

app.Use(async (context, next) =>
{
    await next();

    Log.Information("HTTP {Method} {Path} responded {StatusCode}",
        context.Request.Method,
        context.Request.Path,
        context.Response.StatusCode);
});

// Simple token-based authentication middleware
// This is a placeholder for demonstration purposes.
app.Use(async (context, next) =>
{
    // Allow unauthenticated access to root path
    if (context.Request.Path == "/")
    {
        await next();
        return;
    }

    var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
    var validToken = "TestToken";

    if (authHeader is null || !authHeader.StartsWith("Bearer ") || authHeader.Substring(7) != validToken)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
        return;
    }

    await next();
});

var users = new ConcurrentDictionary<int, User>();
var nextId = 1;

app.MapPost("/users", (User user) =>
{
    if (string.IsNullOrWhiteSpace(user.Name))
        return Results.BadRequest("Name is required.");

    if (user.Name.Length > 100)
        return Results.BadRequest("Name must be 100 characters or less.");

    if (user.Age.HasValue && (user.Age < 0 || user.Age > 100))
        return Results.BadRequest("Age must be between 0 and 100.");

    var id = nextId++;
    users[id] = user;
    return Results.Created($"/users/{id}", new { Id = id, user.Name, user.Age });
})
.WithOpenApi()
.WithDescription("Creates a new user. Requires a valid name and optional age.");

app.MapGet("/users", () =>
    users.Select(u => new { Id = u.Key, u.Value.Name, u.Value.Age })
)
.WithOpenApi()
.WithDescription("Returns a list of all users.");

app.MapGet("/users/{id:int}", (int id) =>
    users.TryGetValue(id, out var user)
        ? Results.Ok(new { Id = id, user.Name, user.Age })
        : Results.NotFound()
)
.WithOpenApi()
.WithDescription("Returns a user by their unique ID.");

app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
    if (string.IsNullOrWhiteSpace(updatedUser.Name))
        return Results.BadRequest("Name is required.");

    if (updatedUser.Name.Length > 100)
        return Results.BadRequest("Name must be 100 characters or less.");

    if (updatedUser.Age.HasValue && (updatedUser.Age < 0 || updatedUser.Age > 100))
        return Results.BadRequest("Age must be between 0 and 100.");

    if (!users.ContainsKey(id))
        return Results.NotFound();

    users[id] = updatedUser;
    return Results.Ok(new { Id = id, updatedUser.Name, updatedUser.Age });
})
.WithOpenApi()
.WithDescription("Updates an existing user by ID.");

app.MapDelete("/users/{id:int}", (int id) =>
{
    return users.TryRemove(id, out _)
        ? Results.NoContent()
        : Results.NotFound();
})
.WithOpenApi()
.WithDescription("Deletes a user by ID.");

app.MapGet("/", () => Results.Ok("Root path"))
.WithOpenApi()
.WithDescription("Root path for health check.");

app.Run();
