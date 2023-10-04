using LazyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyCachePractice.Utilities
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CacheResult : Attribute
    {
        public int TimeOfLifetimeInSeconds;
        public ExpirationMode ExpirationMode;
        public CacheResult(int timeOfLifetimeInSeconds, ExpirationMode expirationMode)
        {
            TimeOfLifetimeInSeconds = timeOfLifetimeInSeconds;
            ExpirationMode = expirationMode;
        }
    }
}
