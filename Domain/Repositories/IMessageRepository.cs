using Domain.Models;
using Domain.Repositories.Divided;

namespace Domain.Repositories;

public interface IMessageRepository : 
    ICreator<Message>,
    IReader<Message>,
    IUpdater<Message>,
    IDeleter<Message>

{
    
}