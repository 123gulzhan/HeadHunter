using System.Threading.Tasks;
using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;

namespace HeadHunter.Services
{
    public class RoleInitializer
    {
        public static async Task Initialize(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            var roles = new[] {"applicant", "employer"};
            
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}