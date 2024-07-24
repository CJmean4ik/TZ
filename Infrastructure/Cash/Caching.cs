using Domain.Generals;
using System.Text.Json;

namespace Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;

public class Caching : ICaching
{
    private readonly IMemoryCache _cash;

    public Caching(IMemoryCache cash)
    {
        _cash = cash;
    }
    
    public async Task<Result> AddToCashAsync<T, TK>(TK? key, T? value)
    {
        if (value is null || key is null)
            return Result.Failure(new Error(ErrorCodes.ValueNull,
                "The value cannot be empty to add to the cache"));

        await Task.Run(() =>
        {
            var serializedObj = JsonSerializer.Serialize(value);
            _cash.Set(key, serializedObj);
        });
        return Result.Success("The object has been added to the cache");
    }

    public async Task<Result> RemoveFromCashAsync<TK>(TK? key)
    {
        if (key is null)
            return Result.Failure(new Error(ErrorCodes.ValueNull,"Key cannot be empty"));
        
        _cash.Remove(key);
        await Task.CompletedTask;
        return Result.Success($"The data with the key {key} has been deleted from the cache");
    }

    public async Task<ResultT<T?>> ReadFromCashAsync<T, TK>(TK? key)
    {
          if (key is null)
            return Result.Failure<T?>(new Error(ErrorCodes.ValueNull,"Key cannot be empty"));

          var result = await Task.Run(() =>
          {
              string? dataFromCash = _cash.Get<string>(key);
              if (string.IsNullOrWhiteSpace(dataFromCash))
                  return Result.Failure<T>(new Error(ErrorCodes.ValueNull,$"Data by key {key} not found"));

              var deserializedObj = JsonSerializer.Deserialize<T>(dataFromCash);
              return Result.Success<T>(deserializedObj!);
          });

          return result!;
    }
}