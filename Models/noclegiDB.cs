using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Noclegi.Model
{
    public partial class noclegiDB : DbContext
    {
        public noclegiDB(DbContextOptions<noclegiDB> options)
            : base(options)
        {
        }

       // public DbSet<AspNetAdress> AspNetAdress { get; set; }
       // public DbSet<AspNetAdvertisement> AspNetAdvertisement { get; set; }
        //public DbSet<AspNetUsers> AspNetUsers { get; set; }

       
    }
}
