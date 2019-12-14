using System;
using System.Threading.Tasks;

namespace API.Services.Cache
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object responseToCache);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
