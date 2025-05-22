using Microsoft.AspNetCore.Identity;

namespace BlazorWebAppMovies.Data
{
    public class IdentitySeeder
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentitySeeder(
            ILogger<IdentitySeeder> logger,
            IConfiguration config,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            this.logger = logger;
            this.configuration = config;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            if (configuration.GetSection("Admin") == null) throw new Exception("Missing admin user configuration for seeding identity data.");

            var rolesCount = roleManager.Roles.Count();
            if (rolesCount == 0)
            {
                await Task.WhenAll([
                    roleManager.CreateAsync(new IdentityRole("Admin")),
                    roleManager.CreateAsync(new IdentityRole("Customer")),
                ]);
            }

            var usersCount = userManager.Users.Count();
            if (usersCount == 0)
            {
                await userManager.CreateAsync(new IdentityUser
                {
                    UserName = configuration.GetSection("Admin")["UserName"],
                    Email = configuration.GetSection("Admin")["Email"],
                    EmailConfirmed = true,
                }, configuration.GetSection("Admin")["Password"]!);

                var admin = await userManager.FindByEmailAsync(configuration.GetSection("Admin")["Email"]!);
                await userManager.AddToRoleAsync(admin!, "Admin");
            }
        }
    }
}
