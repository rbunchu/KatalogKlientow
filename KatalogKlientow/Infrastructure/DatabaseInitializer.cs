using Bogus;
using Dapper;
using KatalogKlientow.Configuration;
using KatalogKlientow.Models;
using KatalogKlientow.Repositories;
using System;
using System.Data.SqlClient;

namespace KatalogKlientow.Infrastructure
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;
        private readonly IClientRepository _clientRepository;

        public DatabaseInitializer(IAppConfiguration appConfiguration, IClientRepository clientRepository)
        {
            if (appConfiguration == null)
            { 
                throw new ArgumentNullException(nameof(appConfiguration));
            }

            _connectionString = appConfiguration.ConnectionString ?? throw new ArgumentException("ConnectionString is null or empty", nameof(appConfiguration));
            _clientRepository = clientRepository;
        }

        public bool Initialize()
        {
            return EnsureDatabaseAndTableExists(_connectionString);
        }

        public void SeedData()
        {
            var klientFaker = new Faker<Client>("pl")
                .RuleFor(k => k.Nazwa, f => f.Company.CompanyName())
                .RuleFor(k => k.Nip, (f, k) => f.UniqueIndex.ToString("D10"))
                .RuleFor(k => k.Adres, f => f.Address.FullAddress())
                .RuleFor(k => k.NrTel, f => f.Phone.PhoneNumber())
                .RuleFor(k => k.Email, (f, k) => f.Internet.Email(k.Nazwa.Replace(" ", "").ToLowerInvariant()));

            var klients = klientFaker.Generate(20);

            _clientRepository.AddKlients(klients);
        }

        public static bool EnsureDatabaseAndTableExists(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString must be provided", nameof(connectionString));
            }

            var builder = new SqlConnectionStringBuilder(connectionString);
            var database = builder.InitialCatalog;
            if (string.IsNullOrWhiteSpace(database))
            {
                throw new InvalidOperationException("Connection string must contain Initial Catalog (database).");
            }

            var masterBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            bool dbCreated = false;
            bool tableCreated = false;

            using (var conn = new SqlConnection(masterBuilder.ConnectionString))
            {
                conn.Open();
                var exists = conn.QuerySingle<int>("SELECT CASE WHEN DB_ID(@db) IS NULL THEN 0 ELSE 1 END", new { db = database });
                if (exists == 0)
                {
                    conn.Execute($"CREATE DATABASE [{database}];");
                    dbCreated = true;
                }
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var tblExists = conn.QuerySingle<int>("SELECT CASE WHEN OBJECT_ID(N'dbo.Klient','U') IS NULL THEN 0 ELSE 1 END");
                if (tblExists == 0)
                {
                    const string createTableSql = @"
                    Create Table Klient (
	                    Id int IDENTITY(1, 1) NOT NULL,
	                    Constraint PK_Id Primary Key CLUSTERED(Id),
	                    Nazwa varchar(254),
	                    Nip char(10) UNIQUE NOT NULL,
	                    Adres varchar(254),
	                    NrTel varchar(15) NULL,
	                    Email varchar(254) NOT NULL,
                    );";
                    conn.Execute(createTableSql);
                    tableCreated = true;
                }
            }

            return dbCreated && tableCreated;
        }
    }
}