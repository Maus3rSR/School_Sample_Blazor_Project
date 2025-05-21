using Microsoft.EntityFrameworkCore;
using BlazorWebAppMovies.Models;

namespace BlazorWebAppMovies.Data
{
    public class BlazorWebAppDbContext : DbContext
    {
        public BlazorWebAppDbContext(DbContextOptions<BlazorWebAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
    }
}
