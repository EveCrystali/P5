using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class DataSeeder
{
    public static async Task SeedData(UserManager<IdentityUser> userManager)
    {
        // Vérifiez si l'utilisateur existe
        var user = await userManager.FindByEmailAsync("admin@email.com");
        if (user == null)
        {
            // Créez l'utilisateur s'il n'existe pas
            var newUser = new IdentityUser
            {
                UserName = "admin@email.com",
                Email = "admin@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false
            };
            await userManager.CreateAsync(newUser, "9vBZBB.QH83GeE.");
        }
    }
}