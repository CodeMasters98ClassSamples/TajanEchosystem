namespace Tajan.Captcha.API.Contracts;

public interface ICaptchaService
{
    string Generate();

    bool Validate();
}
