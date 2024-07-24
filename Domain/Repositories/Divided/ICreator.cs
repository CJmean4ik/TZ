using Domain.Generals;

namespace Domain.Repositories.Divided;

public interface ICreator<T>
{
    Task<ResultT<T>> CreateAsync(T entity);
}