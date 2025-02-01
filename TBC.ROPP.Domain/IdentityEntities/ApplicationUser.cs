using Microsoft.AspNetCore.Identity;

namespace TBC.ROPP.Domain.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public static ApplicationUser Create(string userName, string email, string phoneNumber)
    {
        return new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true
        };
    }
}