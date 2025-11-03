using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebAPIDemo.Controllers;
using Newtonsoft.Json;
using WebAPIDemo.Data.UnitOfWork;
using WebAPIDemo.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IGenericRepository<Klant>, GenericRepository<Klant>>(); (Vervalt met UOW)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<WebAPIDemoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
