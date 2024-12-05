using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Carter;
using FakeApiFarsi.Application.Queries.Todo;
using FakeApiFarsi.Domain;
using FakeApiFarsi.Domain.Address;
using FakeApiFarsi.Domain.Internet;
using FakeApiFarsi.Domain.Order;
using FakeApiFarsi.Domain.Product;
using FakeApiFarsi.Domain.Todo;
using FakeApiFarsi.Domain.User;
using FakeApiFarsi.Infrastructure.Address;
using FakeApiFarsi.Infrastructure.Helpers.TokenHandler;
using FakeApiFarsi.Infrastructure.Internet;
using FakeApiFarsi.infrastructure.Order;
using FakeApiFarsi.infrastructure.Product;
using FakeApiFarsi.infrastructure.Todo;
using FakeApiFarsi.infrastructure.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FakeApiFarsi.Api;

public static class ServicesConfigs
{
    public static IServiceCollection AddFakeApiFarsiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFakeDataRepository<Todo>, TodoFakeDataRepository>();
        builder.Services.AddScoped<IFakeDataRepository<Internet>, InternetFakeDataRepository>();
        builder.Services.AddScoped<IFakeDataRepository<Address>, AddressFakeDataRepository>();
        builder.Services.AddScoped<IFakeDataRepository<Product>, ProductFakeDataRepository>();
        builder.Services.AddScoped<IFakeDataRepository<User>, UserFakeDataRepository>();
        builder.Services.AddScoped<IFakeDataRepository<Order>, OrderFakeDataRepository>();
        builder.Services.AddScoped<TokenHelper>();
        return builder.Services;
    }

    public static IServiceCollection AddPackagesServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddCarter();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<TodoQueryRequest.TodoQuery>());
        return builder.Services;
    }

    public static IServiceCollection AddTokenConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            // Get the version provider
            var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(description.GroupName, new OpenApiInfo()
                {
                    Title = $"Persian API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "API for persian api Website",
                    Contact = new OpenApiContact
                    {
                        Name = "API Developer: MohammadReza Javaheri",
                        Email = "mohammad.r.javaheri@gmail.com",
                    }
                });
            }

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"] ?? "");
            o.SaveToken = true;
            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = new TimeSpan(0, 0, 5),
                RequireExpirationTime = true,
            };
        });

        return builder.Services;
    }

    public static IServiceCollection AddVersioningConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        return builder.Services;
    }
    
    public static IServiceCollection AddCorsConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", c =>
            {
                c.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(builder.Configuration["ClientBaseUrl"]?.Split(",")
                        .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim())
                        .ToArray() ?? [])
                    .AllowCredentials();
            });
        });
        return builder.Services;
    }
}
