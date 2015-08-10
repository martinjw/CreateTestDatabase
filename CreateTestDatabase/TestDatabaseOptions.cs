using System;
using System.IO;
using System.Reflection;

namespace CreateTestDatabase
{
    public class TestDatabaseOptions
    {
        public TestDatabaseOptions(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");

            ConnectionStringName = connectionStringName;
            DatabaseName = "Test" + ConnectionStringName;

            var executingAssembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(executingAssembly.Location) ?? Environment.CurrentDirectory;
            DatabaseFilePath = Path.Combine(directoryName, DatabaseName);
            DataSource = "(LocalDB)\\MSSQLLocalDB";
        }

        /// <summary>
        /// Gets or sets the name of the connection string.
        /// </summary>
        /// <value>
        /// The name of the connection string.
        /// </value>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database. Default is Test+connectionStringName
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the database file path (excluding the .mdf extension). Default is bin directory.
        /// </summary>
        /// <value>
        /// The database file path.
        /// </value>
        public string DatabaseFilePath { get; set; }

        /// <summary>
        /// Gets or sets the data source. Default is (LocalDB)\MSSQLLocalDB = SqlServer 14 localDb. Others: (LocalDB)\v11.0 = SqlServer 2012, .\SqlExpress
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        public string DataSource { get; set; }
    }
}