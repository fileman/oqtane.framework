namespace Oqtane.Database.SqlServer
{
    public class LocalDbDatabase : SqlServerDatabaseBase
    {
        private static string _friendlyName => "Local Database";
        private static string _name => "LocalDB";

        static LocalDbDatabase()
        {
            Initialize(typeof(LocalDbDatabase));
        }

        public LocalDbDatabase() :base(_name, _friendlyName) { }
    }
}