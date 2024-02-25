using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Pokemon.Application.Common.Interfaces;
using Pokemon.Application.Services;
using Pokemon.Infrastructure.Data;
using Pokemon.Infrastructure.Repository;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
Log.Logger = new LoggerConfiguration()
    .MinimumLevel
    .Debug()
    .WriteTo
    .File("logs/pokemon.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnect")));
builder.Services.AddCors(p => p.AddPolicy("pokemon", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "A Pokemon restfull service",
        Version = "v1",
        Description = "This Api enables you perform CRUD operations on any Pokemon of your choice available in the Pokemon database",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Pokemon Api",
            Url = new Uri("https://localhost:44310/swagger/index.html")
        }
    });

});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("pokemon");
app.UseAuthorization();

app.MapControllers();
var apiVersionSet = app.NewApiVersionSet()
    .HasDeprecatedApiVersion(new ApiVersion(0, 0))
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

app.Seed();
app.Run();

