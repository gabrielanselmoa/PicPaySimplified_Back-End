using System.Reflection;
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PicPaySimplified.Application;
using PicPaySimplified.Infrastructure;
using PicPaySimplified.Infrastructure.Data;
using PicPaySimplified.Infrastructure.Http;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../../.env");

builder.Services.AddControllers();

// JWT
var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!);
var issuer = Environment.GetEnvironmentVariable("ISSUER")!;
var audience = Environment.GetEnvironmentVariable("AUDIENCE")!;

// DB CONNECTION
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
var dbName = Environment.GetEnvironmentVariable("DB_NAME")!;

builder.Services.Configure<MongoDbSettings>(
    options =>
    {
        options.ConnectionString = connectionString;
        options.DatabaseName = dbName;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PicPaySimplified API", Version = "v1" });
    
    c.EnableAnnotations();
    c.ExampleFilters();
    
    // Using JWT Security Scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT com prefixo 'Bearer '",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Requires Authentication on Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// JWT Validation and Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Add Infrastructure && Application DI modules
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// Add IHttpClientFactory DI
builder.Services.AddHttpClient<ExternalAuthorizationService>(httpClient =>
{
    httpClient.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();