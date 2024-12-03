using FakeApiFarsi.Application.Queries.Todo;
using FakeApiFarsi.Domain.Todo;
using MediatR;
using Carter;
using FakeApiFarsi.Domain;
using FakeApiFarsi.Domain.Internet;
using FakeApiFarsi.Infrastructure.Internet;
using FakeApiFarsi.infrastructure.Todo;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCarter();  
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<TodoQueryHandler.TodoCommandRequest>());
builder.Services.AddScoped<IFakeDataRepository<Todo>, TodoFakeDataRepository>();
builder.Services.AddScoped<IFakeDataRepository<Internet>, InternetFakeDataRepository>();


builder.Services.AddAuthorization();  


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();  

app.MapCarter();

app.Run();