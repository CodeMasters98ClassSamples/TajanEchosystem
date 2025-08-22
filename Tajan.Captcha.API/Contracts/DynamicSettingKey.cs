namespace Tajan.Captcha.API.Contracts
{

    public enum DynamicSettingKey : byte
    {
        [Value<string>(null)]
        INVALID_USERNAMES,

        [Value<bool>(false)]
        CAPTCHA_USE_REDIS,
    }
}
