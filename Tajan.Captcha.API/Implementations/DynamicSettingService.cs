namespace Tajan.Captcha.API.Implementations;


//public class DynamicSettingService(
//    ISharedDbContext dbContext,
//    ICacheProvider cacheProvider,
//    ILogger<DynamicSettingService> logger)
//    : IDynamicSettingService
//{
//    public async Task<T> Get<T>(string key, T defaultValue = default, CancellationToken ct = default)
//    {
//        //cache approach
//        CachedValue<T> cachedValue = await cacheProvider.GetAsync<T>(CacheKey.DynamicSetting(key), ct);
//        if (cachedValue is not null && cachedValue.IsValid)
//        {
//            logger.LogInformation($"DSS LookUp : {key} = {cachedValue.Value.ToString()}");
//            return cachedValue;
//        }

//        //db approach
//        try
//        {
//            DynamicSetting dynamicSetting = await readOnlyDbContext.Set<DynamicSetting>()
//                .FirstOrDefaultAsync(s => s.Key == key, ct);

//            if (dynamicSetting is not null && dynamicSetting.IsValid)
//            {
//                T dataBaseValue = dynamicSetting.GetValue<T>();
//                await cacheProvider.SetAsync(CacheKey.DynamicSetting(key), dataBaseValue, ct);

//                logger.LogInformation($"DSS LocoUp : {key} = {dataBaseValue.ToString()}");
//                return dataBaseValue;
//            }
//        }
//        catch (Exception ex)
//        {
//            logger.LogError($"get dynamic setting from database failed due to exception : {ex.Message}");
//        }

//        //default approach
//        logger.LogInformation($"DSS LookUp : {key} = {defaultValue.ToString()}");
//        return defaultValue;
//    }

//    public Task<T> Get<T>(DynamicSettingKey key, CancellationToken ct = default)
//        => Get(key.ToString(), key.GetValue<T>(), ct);


//    public async Task Set<T>(string key, T value, CancellationToken ct = default)
//    {
//        await cacheProvider.RemoveAsync(CacheKey.DynamicSetting(key), ct);

//        DynamicSetting dynamicSetting = await dbContext.Set<DynamicSetting>()
//            .FirstOrDefaultAsync(s => s.Key == key, ct);

//        if (dynamicSetting is null)
//        {
//            logger.LogError("try to set value of a dynamic setting that does not exists in database");
//            throw Error.Exception(MessageResource.Error);
//        }

//        dynamicSetting.SetValue(value);
//        await dbContext.SaveChangesAsync(ct);
//    }

//    public Task Set<T>(DynamicSettingKey key, T value, CancellationToken ct = default)
//        => Set(key.ToString(), value, ct);



//    public async Task ResetCache(CancellationToken ct = default)
//    {
//        CacheKey[] settingskeys = await readOnlyDbContext.Set<DynamicSetting>()
//            .Select(x => CacheKey.DynamicSetting(x.Key))
//            .ToArrayAsync(ct);

//        await cacheProvider.RemoveAsync(settingskeys);
//    }

//    public async Task<object> GetAllCahcetSettings(CancellationToken ct = default)
//    {
//        List<CacheKey> keys = await readOnlyDbContext.Set<DynamicSetting>()
//            .Select(x => CacheKey.DynamicSetting(x.Key))
//            .ToListAsync(ct);

//        var values = await cacheProvider.GetAsync<object>(keys, ct);

//        Dictionary<string, object> settingsDict = [];

//        for (int i = 0; i < keys.Count; i++)
//            if (values[i].IsValid)
//                settingsDict.Add(keys[i].Value, values[i].Value);

//        return settingsDict;
//    }
//}

