// Startup Class

using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // For building the web application.

// Add services to the container.

builder.Services.AddControllers();

#region Service Configuration
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); // used for automatic documentation
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
}); // Registering the database context service.
#endregion

var app = builder.Build();

#region Middleware Configuration
// Configure the HTTP request pipeline. // Middleware configuration.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

//app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS.
#endregion

app.MapControllers();

app.Run();
