using Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Tests;

public class CachingTest
{
    [Fact]
    public async Task AddAndGetDataFromCache_SerializeDataAndSeedCash_ReturnDeserializeDataFromCash()
    {
        //Arrange 
        var cacheModel = new CacheModelTest
        {
            Id = 5,
            Name = "Stas"
        };
        string key = "320rt2opmklj";
       
        //Act
        var caching = new Caching(new MemoryCache(new MemoryCacheOptions())); 
        var resultAdd = await caching.AddToCashAsync(key,cacheModel);
        var resultGet = await caching.ReadFromCashAsync<CacheModelTest,string>(key);
        var resultCacheModel = resultGet.Value!;
        
        
        //Assert
        Assert.False(resultAdd.IsFailure);
        Assert.False(resultGet.IsFailure);
        Assert.NotNull(resultCacheModel);
        Assert.Equal(cacheModel.Id, resultCacheModel.Id);
        Assert.Equal(cacheModel.Name, resultCacheModel.Name);
    }
}