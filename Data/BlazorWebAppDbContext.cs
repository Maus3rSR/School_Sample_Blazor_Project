using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlazorWebAppMovies.Models;

namespace BlazorWebAppMovies.Data
{
    public class BlazorWebAppDbContext : IdentityDbContext
    {
        public BlazorWebAppDbContext(DbContextOptions<BlazorWebAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
    }
}
