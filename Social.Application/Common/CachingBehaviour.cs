using Azure.Core;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Social.Application.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Social.Application.Common
{
    public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheable
    {
        private readonly ILogger<CachingBehaviour<TRequest, TResponse>> _logger;
        private readonly IDistributedCache _cache;

        public CachingBehaviour(ILogger<CachingBehaviour<TRequest, TResponse>> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;

            if (request.BypassCache)
            {
                return await next();
            }

            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                if (response != null)
                {
                    var slidingExpiration = request.SlidingExpirationInMinutes == 0 ? 30 : request.SlidingExpirationInMinutes;
                    var absoluteExpiration = request.AbsoluteExpirationInMinutes == 0 ? 60 : request.AbsoluteExpirationInMinutes;
                    var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));

                    var serializedData = Encoding.Default.GetBytes(JsonSerializer.Serialize(response));
                    await _cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
                }
                return response;
            }

            var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

            if (cachedResponse != null) {
                //bug here 
                response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse))!;
                _logger.LogInformation("fetched from cache with key : {CacheKey}", request.CacheKey);
                _cache.Refresh(request.CacheKey);
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation("added to cache with key : {CacheKey}", request.CacheKey);
            }
            return response;
        }

    }
}
