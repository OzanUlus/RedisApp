using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApp.Cache
{
    public class RedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }

        public IDatabase GetDatabase(int dbIndex) 
        {
          return _connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}
