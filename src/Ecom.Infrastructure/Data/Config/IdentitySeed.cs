using Ecom.Core.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Test",
                    Email = "test@test.com",
                    UserName = "Test",
                    Address = new Address
                    {
                        FirstName = "Test",
                        LastName = "Test",
                        City = "Test",
                        State = "Test",
                        Street = "Test",
                        ZipCode = "123"
                    }
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}
