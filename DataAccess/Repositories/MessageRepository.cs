using Domain.Generals;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ChatContext _db;

    public MessageRepository(ChatContext db)
    {
        _db = db;
    }

    public async Task<ResultT<Message>> CreateAsync(Message entity)
    {
        await _db.Messages.AddAsync(entity);
        await _db.SaveChangesAsync();
        return Result.Success(entity,"Message has been saved");
    }

    public async Task<ResultT<List<Message>>> ReadListAsync(int page, int limit)
    {
        var message = await _db.Messages
          .AsNoTracking()
          .Skip((page - 1) * limit)
          .Take(limit)
          .ToListAsync();

        return Result.Success(message, $"Messages received, count: {message.Count}");
    }

    public async Task<Result> UpdateAsync(Message entity)
    {
        Message? oldMessage = await _db.Messages
                                 .Where(w => w.Id == entity.Id)
                                 .FirstOrDefaultAsync();

        if (oldMessage is null)
            return Result.Failure<Message>(new Error(ErrorCodes.NotFounded,
                                        $"Messages by id: {entity.Id} not founded for update"));


        _db.Entry(oldMessage).Property(p => p.Text).CurrentValue = entity.Text;
        _db.Entry(oldMessage).Property(p => p.Text).IsModified = true;

        await _db.SaveChangesAsync();
        return Result.Success("Message text has been updated");
    }

    public async Task<Result> DeleteAsync(Message entity)
    {
        var message = await _db.Messages.FirstOrDefaultAsync(w => w.Id == entity.Id);

        if (message is null)
            return Result.Failure<Message>(new Error(ErrorCodes.NotFounded,
                                        $"Message by id: {entity.Id} not founded for delete"));

        _db.Messages.Remove(message);
        await _db.SaveChangesAsync();
        return Result.Success("Message has been deleted");
    }
}