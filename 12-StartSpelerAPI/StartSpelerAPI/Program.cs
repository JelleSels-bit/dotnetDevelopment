

using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StartSpelerAPI.Data.UnitOfWork;
using StartSpelerAPI.Helpers;
using StartSpelerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
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

//Service voor identity en dbcontext
builder.Services.AddIdentity<Gebruiker, IdentityRole>()
    .AddEntityFrameworkStores<StartspelerAPIContext>();

builder.Services.Configure<IdentityOptions>(options =>
{

    // Password settings. 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings. 
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Voorkom redirects naar een loginpagina wanneer de gebruiker niet is geauthenticeerd
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401; // Unauthorized
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Niet geauthenticeerd. Log in om toegang te krijgen.\"}");
        };

        // Voorkom redirects naar een access denied pagina wanneer de gebruiker geen toegang heeft
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403; // Forbidden
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Toegang geweigerd. U heeft niet de juiste rechten.\"}");
        };
    });

Token.mySettings = new MySettings
{
    Secret = (builder.Configuration["MySettings:Secret"] ?? "d4f.5E6a7-8b9c-0d1e-WfGl1m-4h5i6j7k8l9m").ToCharArray(),
    ValidIssuer = builder.Configuration["MySettings:ValidIssuer"] ?? "https://localhost:7055",
    ValidAudience = builder.Configuration["MySettings:ValidAudience"] ?? "https://localhost:7055"
};

builder.Configuration.GetRequiredSection(nameof(MySettings)).Bind(Token.mySettings);

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
});


builder.Services.AddDbContext<StartspelerAPIContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection")));



// Service voor UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IdentitySeeding>();

// Service voor Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeding>();
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await seeder.RoleSeedingAsync(roleManager);
}

app.Run();
