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

        if (!existUser)
            return Result.Failure<User>(new Error(ErrorCodes.NotFounded, $"User by name: {entity.Name} not found"));

        return Result.Success<User>(entity, "User found!");
    }

    public async Task<ResultT<List<User>>> ReadListAsync(int page, int limit)
    {
        var users = await _db.Users
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return Result.Success(users, $"Users received, count: {users.Count}");
    }

    public async Task<Result> UpdateAsync(User entity)
    {
        User? oldChat = await _db.Users
                                 .Where(w => w.Id == entity.Id)
                                 .FirstOrDefaultAsync();

        if (oldChat is null)
            return Result.Failure<Chat>(new Error(ErrorCodes.NotFounded,
                                        $"User by id: {entity.Id} not founded for update"));


        _db.Entry(oldChat).Property(p => p.Name).CurrentValue = entity.Name;
        _db.Entry(oldChat).Property(p => p.Name).IsModified = true;
        await _db.SaveChangesAsync();

        return Result.Success("User name has been updated");
    }

    public async Task<Result> DeleteAsync(User entity)
    {
        var user = await _db.Users.FirstOrDefaultAsync(w => w.Id == entity.Id);

        if (user is null)
            return Result.Failure<Chat>(new Error(ErrorCodes.NotFounded,
                                        $"User by id: {entity.Id} not founded for update"));

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return Result.Success("User has been deleted");
    }

    public async Task<ResultT<User>> GetUserByNameAsync(string name)
    {
        var user = await _db.Users.Where(w => w.Name == name).FirstOrDefaultAsync();

        if (user is null)
            return Result.Failure<User>(new Error(ErrorCodes.ValueNull, $"User by name: {name} not founded"));

        return Result.Success<User>(user, "User successfully founded");
    }
}