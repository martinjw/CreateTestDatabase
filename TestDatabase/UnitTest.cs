using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using CreateTestDatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestDatabase.EF;

namespace TestDatabase
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            //no special code, because we used AssemblyInitialize
            using (var context = new ProductContext())
            {
                var banana = new Product { Name = "Banana", PricePerUnit = 1 };
                context.Products.Add(banana);
                context.SaveChanges();

                var con = context.Database.Connection;
                Assert.AreEqual("(LocalDB)\\MSSQLLocalDB", con.DataSource);
                Assert.AreEqual("TestProductContext", con.Database);
            }
        }

        [TestMethod]
        public void TestCustomDatabaseName()
        {
            //you can also create a new database per test (it's not very fast)
            //here we customise the database name and put the file in %TEMP%
            var options = new TestDatabaseOptions("ProductContext")
                          {
                              DatabaseName = "Test",
                              DatabaseFilePath = Path.Combine(Path.GetTempPath(), "Test"),
                              //DataSource = @".\SqlExpress", //if this is installed
                          };

            var td = new CreateTestDatabase.TestDatabase(options);
            td.CreateDatabase();
            td.InitConnectionString();
            //if you've already done this with AssemblyInitialize, EF has been initialized and the connection string is different

            //NB: this isn't the connection string in app.config
            var connectionString = ConfigurationManager.ConnectionStrings["ProductContext"].ConnectionString;

            using (var con = new SqlConnection(connectionString))
            {
                Assert.AreEqual("(LocalDB)\\MSSQLLocalDB", con.DataSource);
                Assert.AreEqual("Test", con.Database);
            }
        }
    }
}