// Startup Class

using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddCors(); // Adding controller services to the application.
builder.Services.AddScoped<ITokenService, TokenService>(); // Registering the token service with scoped lifetime (only for http request that need it).   
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
       var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("TokenKey not found - Program.cs");
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
           ValidateIssuer = false,
           ValidateAudience = false
       }; 
    });
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
app.UseCors(opts => 
    opts.AllowAnyHeader().
    AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication(); // Adding authentication middleware to the pipeline.
app.UseAuthorization(); // Adding authorization middleware to the pipeline.
app.MapControllers();

app.Run();
