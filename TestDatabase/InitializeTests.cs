using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestDatabase.EF;
using TestDatabase.Migrations;

namespace TestDatabase
{
    [TestClass]
    public class InitializeTests
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            const string connectionStringName = "ProductContext";

            CreateTestDatabase.TestDatabase.Create(connectionStringName);

            //optional - run Entity Framework migrations
            var migrate = new MigrateDatabaseToLatestVersion<ProductContext, Configuration>(connectionStringName);
            using (var dbContext = new ProductContext())
            {
                migrate.InitializeDatabase(dbContext);
            }
        }
    }
}
