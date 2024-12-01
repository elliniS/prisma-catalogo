using PrismaCatalogo.Api.Context;
using FluentValidation.AspNetCore;
using FluentValidation;
using PrismaCatalogo.Api.Validations;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Repositories;
using PrismaCatalogo.Api.DTOs.Mappings;
using PrismaCatalogo.Api.Logging;
using PrismaCatalogo.Api.Filters;
using PrismaCatalogo.Validations;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(op =>
{
    op.Filters.Add(typeof(ExceptionFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<TamanhoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoriaValidator>();


builder.Services.AddScoped<ITamanhoRepository, TamanhoRepository>();
builder.Services.AddScoped<ICorRepository, CorRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(TamanhoDTOMapping));
builder.Services.AddAutoMapper(typeof(CorDTOMapping));
builder.Services.AddAutoMapper(typeof(CategoriaDTOMapping));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

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
