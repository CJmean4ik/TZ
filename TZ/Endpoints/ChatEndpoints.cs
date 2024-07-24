using Api.Contracts;
using Api.Requests;
using Api.Shared;
using Application.Requests;
using DataAccess;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class ChatEndpoints
{
    public static void AddChatEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var chatRoutes = routeBuilder.MapGroup("api/");

        chatRoutes.MapGet("chat/by/name", GetChatByNameAsync);
        chatRoutes.MapGet("chat/by/name/include/messages", GetChatByNameAndIncludeMessagesAsync);
        chatRoutes.MapGet("chats/", GetLimitChatsAsync);
        chatRoutes.MapPost("chat/", CreateChatAsync);
        chatRoutes.MapDelete("chat/", RemoveChatAsync);
        chatRoutes.MapPut("chat/", UpdateChatAsync);
    }

    private static async Task<IResult> GetChatByNameAsync(
        [FromQuery]string name, 
        [FromServices]ChatServiceParameters services)
    {
        var chat = await services.ChatRepository.GetChatByNameAsync(name);

        if (chat.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:chat));
        }
        
        var chatResponce = new ChatResponce
        {
            Id = chat.Value.Id,
            Title = chat.Value.Name,
        };

        var successResponce = services.ResponceFactory.CreateSuccessResponce(chat,chatResponce);
        return Results.Json(successResponce);
    }
    private static async Task<IResult> GetChatByNameAndIncludeMessagesAsync(
            [FromServices]ChatServiceParameters services,
            [FromQuery]string name,
            [FromQuery]int page = 1,
            [FromQuery]int limit = 50)
    {
        var chat = await services.ChatRepository.GetChatByNameAsync(name, true, page, limit);

        if (chat.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:chat));
        }

        var chatResponce = new ChatResponce
        {
            Id = chat.Value!.Id,
            Title = chat.Value.Name,
            WhoCreateChat = new UserResponce
            {
                Id = chat.Value.WhoCreated!.Id,
                Name = chat.Value.WhoCreated.Name
            },
            Messages = chat.Value.Messages.
                Select(s => new MessageResponce{Id = s.Id, Content = s.Text})
                .ToList()
        };
        
        
        var successResponce = services.ResponceFactory.CreateSuccessResponce(chat,chatResponce);
        return Results.Json(successResponce);
    }
    private static async Task<IResult> GetLimitChatsAsync(
        [FromServices]ChatServiceParameters services,
        [FromQuery]int page = 1,
        [FromQuery]int limit = 10)
    {
        var result = await services.ChatRepository.ReadListAsync(page, limit);

        if (result.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:result));
        }

        if (result.Value!.Count == 0)
            return Results.Json("empty list");

        var chats = result.Value.Select(chat => new ChatResponce
        {
            Id = chat.Id,
            Title = chat.Name,
        });
        
        var successResponce = services.ResponceFactory.CreateSuccessResponce(result,chats);
        return Results.Json(successResponce);
    }

    private static async Task<IResult> CreateChatAsync(
        [FromBody] ChatRequest request,
        [FromServices]ChatServiceParameters services)
    {     
        var chat = Chat.Create(Guid.NewGuid(),request.ChatName, null, request.UserId);
        
        if (chat.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:chat));
        }
            
        
        var result = await  services.ChatRepository.CreateAsync(chat.Value!);
        if (result.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:result));
        }
        
         
        var successResponce = services.ResponceFactory.CreateSuccessResponce<Chat>(result);
        return Results.Json(successResponce);
    }

    private static async Task<IResult> RemoveChatAsync(
        [FromBody] ChatRequest request,
        [FromServices]ChatServiceParameters services)
    {
        var chat = Chat.Create(Guid.NewGuid(),request.ChatName!);
        if (chat.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:chat));
        }
        
        var result = await services.ChatRepository.DeleteAsync(chat.Value!);
        if (result.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:result));
        }
        
        var successResponce = services.ResponceFactory.CreateSuccessResponce<Chat>(result);
        return Results.Json(successResponce);
    }

    private static async Task<IResult> UpdateChatAsync(
       [FromBody] UpdateChatRequest request,
       [FromServices] ChatServiceParameters services)
    {
        var chat = Chat.Create(request.ChatId, request.NewName);
        if (chat.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:chat));
        }

        var result = await services.ChatRepository.UpdateAsync(chat.Value!);
        if (result.IsFailure)
        {
            services.Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Results.Json(services.ResponceFactory.CreateErrorResponce(result:result));
        }
        var successResponce = services.ResponceFactory.CreateSuccessResponce<Chat>(result,chat.Value!);
        return Results.Json(successResponce);
    }
}