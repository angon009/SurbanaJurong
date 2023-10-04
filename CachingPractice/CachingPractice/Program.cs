using System.Runtime.Caching;

MemoryCache memoryCache = new MemoryCache("TestCache");

string value1 = "Hello World from Cache";
string value2 = "Hello Universe from Cache";

CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
cacheItemPolicy.SlidingExpiration = TimeSpan.FromMinutes(10);

memoryCache.Add("TestCacheKey1", value1, cacheItemPolicy);
memoryCache.Add("TestCacheKey2", value2, cacheItemPolicy);

string cachedValue = memoryCache.Get("TestCacheKey1") as string;

Console.WriteLine("Cache from Key 1: " + cachedValue);

cachedValue = memoryCache.Get("TestCacheKey2") as string;

Console.WriteLine("Cache from Key 2 : " + cachedValue);

memoryCache.Remove("TestCacheKey");
