using Domain.Generals;

namespace Domain.Repositories.Divided;

public interface IReader<T>
{
    Task<ResultT<List<T>>> ReadListAsync(int page, int limit);
}