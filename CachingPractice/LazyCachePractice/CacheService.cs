using LazyCache;
using LazyCachePractice.Utilities;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LazyCachePractice
{
    public class CacheService
    {
        private readonly IAppCache _appCache;
        private readonly RetryPolicy<string> _retryPolicy;
        public CacheService(IAppCache appCache)
        {
            _appCache = appCache;

            //Defining retry policy
            _retryPolicy = Policy<string>
             .Handle<Exception>()
             .Retry(3, (exception, retryCount) =>
             {
                 Console.WriteLine($"Exception caught, retrying ({retryCount}/3)");
             });
        }

        #region Simplified API
        /*
         LazyCache provides a more developer-friendly and simplified API compared to MemoryCache. 
         It's designed to make caching operations easier to work with, especially for common use cases.
         */
        public string GetCachedData()
        {
            return _appCache.GetOrAdd("cacheKey", () =>
            {
                // Expensive operation to fetch data
                return FetchDataFromDatabase();
            });
        }
        /*
         In this example This GetOrAdd() method simplifies the process of caching data. If the data is already 
         in the cache it will be returned, if it's not then the specified delegate will fetch data from 
         the database, cache it and then will be returned.
         */
        #endregion

        #region Simpler Cache Key Management
        /*
         LazyCache simplifies cache key management by allowing you to use method names or lambda expressions 
         as cache keys. This can reduce the potential for key naming conflicts and make your code more 
         maintainable.
         */
        public string GetCachedData(int id)
        {
            return _appCache.GetOrAdd($"GetCachedDat_{id}", () =>
            {
                return FetchDataByIdFromDatabase(id);
            });
        }
        /*
         In this example, the cache key incorporates the method name and the input parameter to ensure uniqueness.
         We can also use reflection to get the method name and that would be more accurate.
         */
        public string GetCachedDataV2(int id)
        {
            return _appCache.GetOrAdd(GetType().FullName + nameof(GetCachedDataV2) + id, () =>
            {
                return FetchDataByIdFromDatabase(id);
            });
        }
        /* In this example reflection is used to constuct an unique cache key automatically. This combination
           ensures that each method call with different parameters gets its own distinct cache key.
         * You don't need to manually manage or construct cache keys, reducing the chance of accidentally 
           using the same key for different purposes.
         * The cache key is generated based on the method name and the input parameter, which helps maintain 
           clarity in your code and simplifies key management.
         * If you have multiple methods with similar names but different parameters, this approach guarantees 
           unique cache keys for each combination.
         */
        #endregion

        #region Automatic Cache Item Creation
        /* LazyCache has a feature that automatically handles the creation of cache items when 
           they don't exist in the cache. You don't need to write additional code to check if a 
           specific item is already in the cache and add it if it's not present
        */
        public string GetCachedDataAutomatically(int id)
        {
            return _appCache.GetOrAdd($"GetDataById_{id}", () =>
            {
                return FetchDataByIdFromDatabase(id);
            });
        }
        /* In this example, GetOrAdd to attempt to retrieve data from the cache based on the 
           provided id. If the data doesn't exist in the cache, it fetches the data from a source
           and adds it to the cache.
         */
        #endregion

        #region Built-in Expiration Handling
        /* 
         * LazyCache allows you to specify how long a cached item should remain valid before it 
           automatically expires. This duration is typically set in terms of seconds, minutes, 
           hours, etc.
         * In addition to absolute expiration, LazyCache supports sliding expiration. With sliding 
           expiration, the cache item's timer is reset each time it's accessed. If the item is not 
           accessed within the specified time frame, it expires and is removed from the cache.
         * Once a cached item reaches its expiration or sliding expiration time, LazyCache takes 
           care of automatically removing it from the cache. You don't need to write additional 
           code to clean up expired items.
         */
        public string GetCachedDataWithExpiration(int id)
        {
            DateTimeOffset absoluteExpiration = DateTimeOffset.Now.AddSeconds(10);

            return _appCache.GetOrAdd($"CachedDataWithExpiration_{id}", () =>
            {
                return FetchDataByIdFromDatabase(id);
            }, absoluteExpiration);
        }
        /* In this example, cached data will be cleared from Cache after 10 seconds whether it is accessed or not.*/
        public string GetCachedDataWithSlidingExpiration(int id)
        {
            TimeSpan slidingExpiration = TimeSpan.FromSeconds(10);

            return _appCache.GetOrAdd($"CachedDataWithSlidingExpiration_{id}", () =>
            {
                return FetchDataByIdFromDatabase(id);
            }, slidingExpiration);
        }
        /* In this example, cached data will be cleared from Cache after 10 seconds if it is not accessed or if its 
           accessed then another 10 seconds expiration time will be reset for that cache.
        */
        #endregion

        #region Retry Policies
        /* 
         * Handling Exceptions: Retry policies in LazyCache allow you to specify how to handle exceptions that might occur
           during cache operations, such as reading from or writing to the cache.
         * Retry Strategies: You can define retry strategies that determine whether and how to retry a cache operation 
           when an exception occurs. For example, you can specify that the operation should be retried a certain number of 
           times or with a specific delay between retries.
         * Caching Strategies: Retry policies also enable you to define caching strategies. This means you can decide what 
           to do when an exception occurs while trying to fetch data to be cached. You might choose to retry the operation,
           fetch new data, or return cached data (if available) even when there's an error.
         * Custom Error Handling: You can implement custom error handling logic using retry policies, tailoring the 
           behavior of cache operations to your application's specific needs. This allows you to handle cache-related 
           exceptions gracefully.
         */
        public string GetCachedDataWithRetry(int id)
        { 
            return _retryPolicy.Execute(() =>
            { 
                if (DateTime.Now.Second % 2 == 0)
                {
                    throw new Exception("Cache operation failed");
                } 
                return _appCache.GetOrAdd($"CachedDataWithRetry_{id}", () =>
                {
                    return FetchDataByIdFromDatabase(id);
                });
            });
        }
        /* In this example, the GetCachedDataWithRetry method will throw exceptions, and the retry policy will handle 
           them by retrying the cache operation up to 3 times*/
        #endregion

        #region Decorative Caching
        /* 
         * With LazyCache, you can use caching attributes to "decorate" your methods. By adding these attributes to a 
           method, you specify caching behavior for that method without changing the method's core logic.
         * These attributes allow you to define caching settings such as cache keys, expiration policies, and more directly 
           on the method they decorate.
         * Using caching attributes is convenient because it abstracts the caching logic away from the method 
           implementation. This means you can easily add or modify caching behavior without altering the method's code.
        */
        [CacheResult(3, ExpirationMode.LazyExpiration)]
        public string GetCachedDataAttribute(int id)
        {
            return _appCache.GetOrAdd($"GetCachedDat_{id}", () =>
            {
                return FetchDataByIdFromDatabase(id);
            });
        }
        #endregion

        #region Pluggable Storage Backends
        /*
         * LazyCache is designed in a way that allows you to easily switch between different storage mechanisms or backends 
           for your cached data. This means you can choose where your data is stored based on your application's 
           requirements.
         * One of the notable options you can use with LazyCache is distributed caching systems like Redis. Distributed 
           caches store data across multiple servers or nodes, making it suitable for scenarios where you need to share 
           cached data across multiple instances of your application or even across different applications.
         */
        #endregion

        private string FetchDataFromDatabase()
        {
            return "Cached Data";
        }
        private string FetchDataByIdFromDatabase(int id)
        {
            return $"Data for Id {id}";
        }
    }
}
