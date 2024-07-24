using Domain.Generals;

namespace Domain.Repositories.Divided;

public interface IDeleter<T>
{
    Task<Result> DeleteAsync(T entity);
}