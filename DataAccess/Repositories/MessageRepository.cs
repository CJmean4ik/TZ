using Domain.Generals;
using Domain.Models;
using Domain.Repositories;

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

    public Task<ResultT<List<Message>>> ReadListAsync(int page, int limit)
    {
        //For future implementation
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Message entity)
    {
        //For future implementation
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Message entity)
    {
        //For future implementation
        throw new NotImplementedException();
    }
}