using Domain.Generals;

namespace Domain.Repositories.Divided;

public interface IUpdater<T>
{
    Task<Result> UpdateAsync(T entity);
}