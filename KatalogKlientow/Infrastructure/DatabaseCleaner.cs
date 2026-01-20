using Dapper;
using KatalogKlientow.Configuration;
using System;
using System.Data.SqlClient;

namespace KatalogKlientow.Infrastructure
{
    public class DatabaseCleaner
    {
        private readonly IAppConfiguration _appConfiguration;

        public DatabaseCleaner(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration));
        }

        public void CleanData()
        {
            using (var connection = new SqlConnection(_appConfiguration.ConnectionString))
            {
                connection.Open();
                connection.Execute("TRUNCATE TABLE Klient");
            }
        }
    }
}