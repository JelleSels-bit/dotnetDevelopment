using DemoIdentity;
using DemoIdentity.Data;
using DemoIdentity.Helper;
using DemoIdentity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// We gebruiken de MySettings in Token Class, de Login methode heeft deze nodig
// Token overpompen in MySettings
Token.mySettings = new MySettings
{
    Secret = (builder.Configuration["MySettings:Secret"] ?? "d4f.5E6a7-8b9c-0d1e-WfGl1m-4h5i6j7k8l9m").ToCharArray(),
    ValidIssuer = builder.Configuration["MySettings:ValidIssuer"] ?? "https://localhost:7055",
    ValidAudience = builder.Configuration["MySettings:ValidAudience"] ?? "https://localhost:7055"
};

// MySettings opvullen
builder.Configuration.GetRequiredSection(nameof(MySettings)).Bind(Token.mySettings);

// Add services to the container.
builder.Services.AddControllers();

//
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection")));

builder.Services.AddIdentity<CustomUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();
//

// Bron: https://github.com/devr24/MultipleSecuredApis/blob/main/src/AuthTest/Program.cs
//var audiences = builder.Configuration.GetSection("MySettings:ValidAudiences").Get<string[]>();
var audiences = Token.mySettings.ValidAudiences;

// Authentication toevoegen
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Jwt Bearer toevoegen
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true; // debugging
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.UseSecurityTokenValidators = true; // fix bug 8.0
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = false, // onderdrukken bug 8.0
        //ValidateIssuer = true,
        //ValidateAudience = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        //ValidAudiences = audiences,
        //ValidIssuer = Token.mySettings.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Token.mySettings.Secret))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            return Task.CompletedTask;
        }
    };
});

// Authorisaties
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Superadmin", policy => policy.RequireClaim(ClaimTypes.Role, "superadmin"));
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "user"));
});

builder.Services.AddEndpointsApiExplorer();

/// Swagger
builder.Services.AddSwaggerGen(swagger => {
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AuthTest Web API",
        Description = "Authentication and Authorization in ASP.NET with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter ZONDER Bearer. Dus gewoon je TOKEN: \r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

//

// Identity Seeding
builder.Services.AddTransient<IdentitySeeding>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Belangrijk
app.UseAuthentication();
app.UseAuthorization();
//

app.MapControllers();

// Seeding
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeding>();
    UserManager<CustomUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomUser>>();
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await seeder.IdentitySeedingAsync(userManager, roleManager);
}

app.Run();