using StackExchange.Redis;
using System.Text.Json;

namespace CacheAsideExample.Services
{
    public class RedisCacheAside
    {


        // Redis connection string information
        public static ConnectionMultiplexer Connection => lazyConnection.Value;

        private static Lazy<ConnectionMultiplexer> lazyConnection = 
            new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("localhost:6379");
        });

        IDatabase _cache;
        public RedisCacheAside()
        {
            _cache = Connection.GetDatabase();
        }


        public async Task<T> Get<T>(string key, Func<Task<T>> GetFromDb)
        {
            T result;
            if (_cache.KeyExists(key))
            {
                var cachedData = await _cache.StringGetAsync(key);
                //Get the result from the cache
                result = JsonSerializer.Deserialize<T>(cachedData);
            }
            else
            {
                //Get from database
                result = await GetFromDb();
                //Set in cache dictionary
                await _cache.StringSetAsync(key, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(10));
            }
            return result;

        }
        public async Task Invalidate(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
    }
}
