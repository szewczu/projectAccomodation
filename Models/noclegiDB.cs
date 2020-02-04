using Microsoft.EntityFrameworkCore;

namespace Noclegi.Model
{
    public partial class noclegiDB : DbContext
    {
        public noclegiDB(DbContextOptions<noclegiDB> options)
            : base(options)
        {
        }
    }
}
