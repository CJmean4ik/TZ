using Domain.Generals;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ChatContext _db;

    public UserRepository(ChatContext db)
    {
    }

    public async Task<ResultT<User>> CreateAsync(User entity)
    {
        var existUser = await _db.Users.AnyAsync(w => w.Name == entity.Name);
        return null;
    }

    public Task<ResultT<List<User>>> ReadListAsync(int page, int limit)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultT<User>> GetUserByNameAsync(string name)
    {
        var user = await _db.Users.Where(w => w.Name == name).FirstOrDefaultAsync();

        if (user is null)
            return Result.Failure<User>(new Error(ErrorCodes.ValueNull,$"User by name: {name} not founded"));
        
        return Result.Success<User>(user, "User successfully founded");
    }
}