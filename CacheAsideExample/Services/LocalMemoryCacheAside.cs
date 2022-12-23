using NuGet.Packaging.Signing;
using System.Collections.Concurrent;

namespace CacheAsideExample.Services
{
    public class LocalMemoryCacheAside
    {
        static ConcurrentDictionary<string, object> _cache = new();

        public async Task<T> Get<T>(string key, Func<Task<T>> GetFromDb)
        {
            T result;
            
            if (_cache.TryGetValue(key, out object cachedObject))
            {
                //Get the result from the cache
                result = (T)cachedObject;
            }
            else
            {
                //Get from database
                result = await GetFromDb();
                //Set in cache dictionary
                _cache.TryAdd(key, result);
            }
            return result;

        }
        public void Invalidate(string key)
        {
            _cache.TryRemove(key,out var value);
        }
    }
}
