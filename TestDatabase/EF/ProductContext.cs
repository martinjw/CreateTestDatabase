using System.Data.Entity;
using System.Diagnostics;

namespace TestDatabase.EF
{
    public class ProductContext : DbContext
    {
        public ProductContext()
            : base()
        {
            Database.Log = s => Trace.WriteLine(s);
        }

        public DbSet<Product> Products { get; set; }
    }
}
