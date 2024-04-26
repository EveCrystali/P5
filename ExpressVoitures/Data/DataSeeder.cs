using Microsoft.AspNetCore.Identity;

public static class DataSeeder
{
    public static async Task SeedData(UserManager<IdentityUser> userManager)
    {
        // Vérifiez si l'utilisateur existe
        IdentityUser? user = await userManager.FindByEmailAsync("admin@email.com");
        if (user == null)
        {
            var newUser = new IdentityUser { UserName = "admin@email.com", Email = "admin@email.com", EmailConfirmed = true, LockoutEnabled = false };
            await userManager.CreateAsync(newUser, "9vBZBB.QH83GeE.");
        }
    }
}
