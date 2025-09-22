using Microsoft.AspNetCore.Identity;

namespace Tajan.Identity.Infrastructure.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public int VerifyCode { get; set; }
    public DateTime VerifyCodeExpirationDate { get; set; }

    public void CreateOtp()
    {
        Random rnd = new Random();
        VerifyCode = rnd.Next(1000, 9999);
        VerifyCodeExpirationDate = DateTime.Now.AddMinutes(4);
    }
}
