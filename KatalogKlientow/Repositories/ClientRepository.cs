using Dapper;
using KatalogKlientow.Configuration;
using KatalogKlientow.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KatalogKlientow.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IAppConfiguration _appConfiguration;

        public ClientRepository(IAppConfiguration configuration)
        {
            _appConfiguration = configuration;
        }

        public IEnumerable<Client> GetKlients()
        {
            using (var connection = new SqlConnection(_appConfiguration.ConnectionString))
            {
                return connection.Query<Client>("SELECT Id, Nazwa, Nip, Adres, NrTel, Email FROM Klient").ToList();
            }
        }

        public void AddKlients(IEnumerable<Client> klients)
        {
            using (var connection = new SqlConnection(_appConfiguration.ConnectionString))
            {
                connection.Execute("INSERT INTO Klient (Nazwa, Nip, Adres, NrTel, Email) VALUES (@Nazwa, @Nip, @Adres, @NrTel, @Email)", klients);
            }
        }

        public void AddKlient(Client klient)
        {
            using (var connection = new SqlConnection(_appConfiguration.ConnectionString))
            {
                connection.Execute("INSERT INTO Klient (Nazwa, Nip, Adres, NrTel, Email) VALUES (@Nazwa, @Nip, @Adres, @NrTel, @Email)", klient);
            }
        }

        public void UpdateKlient(Client klient)
        {
            using (var connection = new SqlConnection(_appConfiguration.ConnectionString))
            {
                connection.Execute("UPDATE Klient SET Nazwa = @Nazwa, Nip = @Nip, Adres = @Adres, NrTel = @NrTel, Email = @Email WHERE Id = @Id", klient);
            }
        }

        public void DeleteKlient(int id)
        {
            using (var connection = new SqlConnection(_appConfiguration.ConnectionString))
            {
                connection.Execute("DELETE FROM Klient WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
