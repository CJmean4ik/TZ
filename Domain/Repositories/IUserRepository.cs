using Domain.Generals;
using Domain.Models;
using Domain.Repositories.Divided;

namespace Domain.Repositories;

public interface IUserRepository: 
    ICreator<User>,
    IReader<User>,
    IUpdater<User>,
    IDeleter<User>
{
    Task<ResultT<User>> GetUserByNameAsync(string name);
}