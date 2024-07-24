using Api.Contracts;
using Api.Endpoints;
using Api.Hubs;
using Api.Middlewares;
using Api.Shared;
using Application;
using DataAccess;
using Domain.Repositories;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddLogging();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataAccessLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();

builder.Services.AddSingleton<IResponceFactory, ResponceFactory>();
builder.Services.AddScoped<ChatServiceParameters>();

var app = builder.Build();

app.MapHub<ChatHub>("/chat");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.AddChatEndpoints();

app.MapGet("/test/error",async (IUserRepository repository) =>
{
    var result = await repository.GetUserByNameAsync("Stas");
    return Results.Json(result);
});

app.Run();