using Tajan.Captcha.API.Contracts.CacheProvider;
//using WonderfulCaptcha.Cache;

namespace Tajan.Captcha.API.CaptchaProvider;

//public class WonderfulCaptchaCacheProvider(ICacheProvider cacheProvider) //: WonderfulCaptcha.Cache.ICacheProvider
//{
//    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
//        => await cacheProvider.GetAsync<T>(CacheKey.Captcha(cacheKey), cancellationToken);
//    public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken cancellationToken = default)
//    {
//        Func<ValueTask<T>> valueSetter = async () => await ValueTask.FromResult(await setter());
//        return await cacheProvider.GetAsync(CacheKey.Captcha(cacheKey), valueSetter, expiration, cancellationToken);
//    }

//    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
//        => cacheProvider.RemoveAsync(CacheKey.Captcha(key), cancellationToken);
//    public Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
//        => cacheProvider.SetAsync(CacheKey.Captcha(cacheKey), cacheValue, expiration, cancellationToken);
//}
