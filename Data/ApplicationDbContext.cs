using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noclegi.Model;

namespace Noclegi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<IdentityUser> UsersTB { get; set; }
        public DbSet<AspNetAdress> AspNetAdress { get; set; }
        public DbSet<AspNetAdvertisement> AspNetAdvertisement { get; set; }      

    }
}
