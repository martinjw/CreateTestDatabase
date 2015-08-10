using TestDatabase.EF;

namespace TestDatabase.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TestDatabase.EF.ProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestDatabase.EF.ProductContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.Products.AddOrUpdate(p => p.Name,
                new Product { Name = "Apples", PricePerUnit = 0.5m },
                new Product { Name = "Oranges", PricePerUnit = 0.8m });
        }
    }
}
