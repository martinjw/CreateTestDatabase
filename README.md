# Create Test Database

This is a small utility to create temporary databases (in SqlServer localDb or SqlExpress) for **integration tests**.

## MsTest ##

You can also use NUnit, XUnit or any other testing framework.

Initialize it in [AssemblyInitialize] so each test run uses a new copy of the database.

Just pass in the connection string name. 

```C#
[AssemblyInitialize]
public static void AssemblyInit(TestContext context)
{
	const string connectionStringName = "ProductContext";

	TestDatabase.Create(connectionStringName);

    //optional - run Entity Framework migrations
	var migrate = new MigrateDatabaseToLatestVersion<ProductContext, Configuration>(connectionStringName);
	using (var dbContext = new ProductContext())
	{
		migrate.InitializeDatabase(dbContext);
	}
}
```

After the test run, the database is left in your bin directory. Don't leave it open in Visual Studio or SSMS, as it will be locked and your **next** test run may fail.

You can delete the database in [AssemblyCleanup] by calling testDatabase.DropDatabase()

## More control ##

You can change the 

```C#
[AssemblyInitialize]
public static void AssemblyInit(TestContext context)
{
	const string connectionStringName = "ProductContext";

	var options = new TestDatabaseOptions(connectionStringName)
				  {
					  DatabaseName = "Test",
					  DatabaseFilePath = Path.Combine(Path.GetTempPath(), "Test"),
					  DataSource = @".\SqlExpress",
				  };

	var td = new TestDatabase(options);
	td.CreateDatabase();
	td.InitConnectionString();
			
    //optional - run Entity Framework migrations
	var migrate = new MigrateDatabaseToLatestVersion<ProductContext, Configuration>(connectionStringName);
	using (var dbContext = new ProductContext())
	{
		migrate.InitializeDatabase(dbContext);
	}
}
```

## FAQ ##

### Why not just use a localDb or SqlExpress?

The data can get corrupt over many failing test runs. Ideally you'd be deleting/truncating tables and re-seeding them each time.
With a temporary database file, it's a clean slate every time.
When you use Entity Framework, you are can test your migrations every test run (see AssemblyInit code above). 
You probably still want to delete from specific tables between each test, depending on your application.

### Can I use NUnit or XUnit instead of MsTest?

In NUnit, put the initialization in [SetUpFixture]. 
In XUnit, you'll need a ICollectionFixture, and beware of concurrency issues.

### Can I run it in every test, so each test has a separate database?

If you use Entity Framework, no, because the dbContext is created once per AppDomain.
Otherwise, yes, but it'll be slow (you'll be running your DDL each time).

### Integration tests are fragile!

Yes, they are, and your tests may fail occasionally. The biggest problem is opening the temporary database in Visual Studio/ SSMS, so it can't be deleted when the the test next runs.

### It errors because I have no (LocalDB)\MSSQLLocalDB

You haven't got SqlServer Express 14 installed. You can change it (with options, see above) so it points to SqlExpress (DataSource = @".\SqlExpress") or localDb v11 (DataSource = @"(LocalDB)\v11.0")

### Finding out what went wrong?

Some errors are written to Console standard output.