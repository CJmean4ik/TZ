using Domain.Generals;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ChatContext _db;

    public ChatRepository(ChatContext db)
    {
        _db = db;
    }

    public async Task<ResultT<Chat>> CreateAsync(Chat entity)
    {
        var chat = await _db.Chats.Where(w => w.Name == entity.Name).AnyAsync();

        if (chat)
            return Result.Failure<Chat>(new Error(ErrorCodes.AllreadyExist, "Chat name alredy occupied"));

        await _db.Chats.AddAsync(entity);

        await _db.SaveChangesAsync();
        return Result.Success(entity, "Chat has been created");

    }

    public async Task<ResultT<List<Chat>>> ReadListAsync(int page = 1, int limit = 10)
    {
        var chats = await _db.Chats
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return Result.Success(chats, $"Chats received, count: {chats.Count}");
    }

    public async Task<Result> UpdateAsync(Chat entity)
    {
        try
        {
            Chat? oldChat = await _db.Chats
                .Where(w => w.Id == entity.Id)
                .FirstOrDefaultAsync();

            if (oldChat is null)
                return Result
                    .Failure<Chat>(
                        new Error(ErrorCodes.NotFounded,
                               $"Chat by id: {entity.Id} or name: {entity.Name} not founded for update"));

            _db.Entry(oldChat).Property(p => p.Name).CurrentValue = entity.Name;
            _db.Entry(oldChat).Property(p => p.Name).IsModified = true;
            await _db.SaveChangesAsync();

            return Result.Success("Chat name has been updated");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<Result> DeleteAsync(Chat entity)
    {
        var chat = await _db.Chats.FirstOrDefaultAsync(w => w.Id == entity.Id || w.Name == entity.Name);

        if (chat is null)
            return Result.Failure<Chat>(new Error(ErrorCodes.NotFounded,
                          $"Chat by id: {entity.Id} or name: {entity.Name} not founded for update"));

        _db.Chats.Remove(chat);
        await _db.SaveChangesAsync();

        return Result.Success("Chat has been deleted");
    }

    public async Task<ResultT<Chat>> GetChatByNameAsync(string name)
    {
        var chat = await _db.Chats.Where(w => w.Name == name).FirstOrDefaultAsync();

        if (chat is null)
            return Result.Failure<Chat>(new Error(ErrorCodes.ValueNull, $"Chat by name: {name} not founded"));

        return Result.Success(chat, "Chat successfully founded");
    }

    public async Task<ResultT<List<Chat>>> ReadListAsync(bool includeMessages, int page, int limit)
    {
        var chats = await _db.Chats
            .Include(i => i.Messages.Skip((page - 1) * limit).Take(limit))
            .Include(i => i.WhoCreated)
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return Result.Success(chats, $"Chats received, count: {chats.Count}");
    }

    public async Task<ResultT<Chat>> GetChatByNameAsync(string name, bool includeMessages = false, int page = 1, int limit = 50)
    {
        if (includeMessages)
        {
            var chat = await _db.Chats.Where(w => w.Name == name)
                .Include(m => m.Messages.Skip((page - 1) * limit).Take(limit))
                .Include(m => m.WhoCreated)
                .FirstOrDefaultAsync();

            if (chat is null)
                return Result.Failure<Chat>(new Error(ErrorCodes.ValueNull, $"Chat by name: {name} not founded"));

            return Result.Success(chat, "Chat successfully founded");
        }
        var result = await GetChatByNameAsync(name);
        return result;
    }
}