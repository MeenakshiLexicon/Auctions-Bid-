using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WebApplication_Inlamning_P_3.Repository.Interfaces;
using WebApplication_Inlamning_P_3.Repository.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure authentication and authorization
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    // Token validation parameters
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5006",
        ValidAudience = "http://localhost:5006",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkey1234678NewSecretKey!#")),
        ClockSkew = TimeSpan.Zero
    };
});

// Configure policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    // Add other policies as needed based on ClaimTypes.Role or other claim types
});


// Add database and repository services
builder.Services.AddSingleton<IDBContext, DBContext>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ILoanRepo, LoanRepo>();
//builder.Services.AddScoped<IAccountRepo, AccountRepo>();
//builder.Services.AddScoped<ITransactionRepo, TransactionRepo>();


var secretKey = builder.Configuration["Jwt:SecretKey"];
builder.Services.AddScoped<IJWTService>(s => new JWTServiceRepo(secretKey));


// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });

    // Configure Basic Authentication
    c.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme.",
    });
});


//  Request Pipeline- ett långt rör som requesten går igenom.Här lägger vi på funktionalitet
// tex routing, säkerhet(authentication-authorization),swagger mm
var app = builder.Build();

app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
});

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();

