using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace API.Services.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        public class CacheInput
        {
            public string Id { get; set; }
            public object ResponseToCache { get; set; }
        }

        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync<T>(string cacheKey, bool putInsideDictionary, CacheInput objectToCache) where T : class
        {
            if (objectToCache.ResponseToCache == null)
            {
                return;
            }

            var cachedResponse = await GetCachedResponseAsync<T>(cacheKey);

            // No caching has been done on this object
            if (cachedResponse == null)
            {
                string serializeResponse;

                if (putInsideDictionary) {
                    var cacheDictionary = new Dictionary<string, object>();
                    cacheDictionary.Add(objectToCache.Id, objectToCache.ResponseToCache);
                    serializeResponse = JsonConvert.SerializeObject(cacheDictionary);
                } else
                {
                    serializeResponse = JsonConvert.SerializeObject(objectToCache.ResponseToCache);
                }

                // TODO Set a timer so that it always will be removed at some point, otherwise it will just potentially become increasingly huge
                await _distributedCache.SetStringAsync(cacheKey, serializeResponse);
            }

            Type cachedResponseType = cachedResponse.GetType();
            if (cachedResponseType.IsDi)
            {

            }
        }

        public async Task<T> GetCachedResponseAsync<T>(string cacheKey) where T : class
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cachedResponse) ? null : JsonConvert.DeserializeObject<T>(cachedResponse);
        }
    }
}
