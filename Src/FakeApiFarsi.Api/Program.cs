using Asp.Versioning.ApiExplorer;
using Carter;
using FakeApiFarsi.Api;
using FakeApiFarsi.Api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPackagesServices();
builder.AddFakeApiFarsiServices();
builder.AddTokenConfig();
builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);
builder.AddVersioningConfig();
builder.AddCorsConfig();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseMiddleware<ExceptionHandelingMiddleware>();
app.UseMiddleware<LimitMiddleware>();
// app.UseMiddleware<JwtValidationMiddleware>();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();