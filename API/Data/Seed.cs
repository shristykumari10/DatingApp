using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static  async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            if (users == null || users.Count == 0)
            {
                Console.WriteLine("No users found in JSON or deserialization failed.");
                return;
            }


            //if (users == null) return;

            var roles = new List<AppRole>()
            {
                new() { Name = "Member" },
                new() { Name = "Admin"},
                new() { Name = "Moderator" }

            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
               

            }

            



            foreach( var user in users )
            {
                user.Photos.First().IsApproved = true;
                user.UserName = user.UserName!.ToLower();
              var result =   await userManager.CreateAsync(user, "Pa$$w0rd");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user {user.UserName}: {error.Description}");
                    }
                    continue;
                }
                await userManager.AddToRoleAsync(user, "Member");


             }
            var admin = new AppUser
            {
                UserName = "admin",
                KnownAs = "Admin",
                Gender = "",
                city = "",
                country = ""
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin,  [ "Admin", "Moderator"]);



            
        }
    }
}
