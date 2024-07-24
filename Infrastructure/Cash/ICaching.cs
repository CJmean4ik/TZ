using Domain.Generals;

namespace Infrastructure.Cache;

public interface ICaching
{
    Task<Result> AddToCashAsync<T,TK>(TK? key,T? value);
    Task<Result> RemoveFromCashAsync<TK>(TK? key);
    Task<ResultT<T?>> ReadFromCashAsync<T,TK>(TK? key);
}