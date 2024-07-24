using Domain.Generals;
using Domain.Models;
using Domain.Repositories.Divided;

namespace Domain.Repositories;

public interface IChatRepository : 
    ICreator<Chat>,
    IReader<Chat>,
    IUpdater<Chat>,
    IDeleter<Chat>
{
    Task<ResultT<Chat>> GetChatByNameAsync(string name);
    Task<ResultT<List<Chat>>> ReadListAsync(bool includeMessages,int page, int limit);
    Task<ResultT<Chat>> GetChatByNameAsync(string name,bool includeMessages,int page = 1, int limit = 10);
}