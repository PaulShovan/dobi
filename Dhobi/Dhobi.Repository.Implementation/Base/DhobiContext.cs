using System;
using MongoDB.Driver;

namespace Dhobi.Repository.Implementation.Base
{
    public class DhobiContext
    {
        public IMongoDatabase Database;

        public DhobiContext()
        {
            try
            {
                var client = new MongoClient(Properties.DbSettings.Default.ConnectionString);
                Database = client.GetDatabase(Properties.DbSettings.Default.Database);
            }
            catch (Exception exception)
            {
                throw new Exception("Error connecting to DB" + exception);
            }
        }
    }
}
