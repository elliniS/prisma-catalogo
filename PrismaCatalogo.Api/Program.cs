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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using PrismaCatalogo.Api;
using Microsoft.IdentityModel.Tokens;
using PrismaCatalogo.Api.Services.Interfaces;
using PrismaCatalogo.Api.Services;
using Microsoft.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

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

var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<TamanhoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoriaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProdutoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProdutoFilhoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();


builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>(); 
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IImagemService, ImagemService>();


builder.Services.AddAutoMapper(typeof(TamanhoDTOMapping));
builder.Services.AddAutoMapper(typeof(CorDTOMapping));
builder.Services.AddAutoMapper(typeof(CategoriaDTOMapping));
builder.Services.AddAutoMapper(typeof(ProdutoDTOMapping));
builder.Services.AddAutoMapper(typeof(ProdutoFilhoDTOMapping));
builder.Services.AddAutoMapper(typeof(ProdutoFotoDTOMapping));
builder.Services.AddAutoMapper(typeof(ProdutoFilhoFotoDTOMapping));
builder.Services.AddAutoMapper(typeof(UsuarioDTOMapping));



builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalogo", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Insira um Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
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
});



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

app.Run();
