using LazyCache;
using LazyCachePractice;

IAppCache appCache = new CachingService();

var cacheService = new CacheService(appCache);

//Console.WriteLine(cacheService.GetCachedData());
//Console.WriteLine(cacheService.GetCachedData(3));
 

//string cachedData = cacheService.GetCachedDataV2(id);

//Console.WriteLine($"Cached Data for ID {id} : {cachedData}");

//string cachedDataFromCache = appCache.Get<string>
//    ($"CacheService.GetCachedDataV2{typeof(CacheService).FullName}{nameof(CacheService.GetCachedDataV2)}{id}");

//Console.WriteLine(cachedDataFromCache);


//string automaticCacheData = cacheService.GetCachedDataAutomatically(id);
//Console.WriteLine("Automatically : " + automaticCacheData);

//string automaticCachedData2 = cacheService.GetCachedDataAutomatically(id);
//Console.WriteLine("Automatically 2nd : " + automaticCachedData2);

//string cachedDataWithExpiration = cacheService.GetCachedDataWithExpiration(id);
//Console.WriteLine(cachedDataWithExpiration);

//Thread.Sleep(TimeSpan.FromSeconds(15));

//string cachedDataWithSlidingExpiration = cacheService.GetCachedDataWithSlidingExpiration(id);
//Console.WriteLine($"Cached Data with Sliding Expiration for ID {id}: {cachedDataWithSlidingExpiration}");

int id = 123;
string cachedDataWithRetry = cacheService.GetCachedDataWithRetry(id);

// Display the retrieved or newly fetched data
Console.WriteLine($"Cached Data with Retry for ID {id}: {cachedDataWithRetry}");

// Wait for user input before exiting
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();

