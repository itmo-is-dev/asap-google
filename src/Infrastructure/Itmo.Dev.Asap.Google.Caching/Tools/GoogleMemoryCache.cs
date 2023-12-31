using Itmo.Dev.Asap.Google.Caching.Models;
using Itmo.Dev.Asap.Google.Common.Tools;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Itmo.Dev.Asap.Google.Caching.Tools;

public sealed class GoogleMemoryCache : IGoogleMemoryCache
{
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _entryOptions;

    public GoogleMemoryCache(IOptions<GithubCacheConfiguration> options)
    {
        var memoryCacheOptions = new MemoryCacheOptions
        {
            SizeLimit = options.Value.SizeLimit,
            ExpirationScanFrequency = options.Value.ExpirationScanFrequency,
        };

        _cache = new MemoryCache(memoryCacheOptions);

        _entryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = options.Value.EntryAbsoluteExpiration,
            SlidingExpiration = options.Value.EntrySlidingExpiration,
        };
    }

    public bool TryGetValue(object key, [UnscopedRef] out object? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public ICacheEntry CreateEntry(object key)
    {
        return _cache.CreateEntry(key).SetOptions(_entryOptions);
    }

    public void Remove(object key)
    {
        _cache.Remove(key);
    }

    public void Dispose()
    {
        _cache.Dispose();
    }
}