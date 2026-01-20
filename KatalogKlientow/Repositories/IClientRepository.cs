using KatalogKlientow.Models;
using System.Collections.Generic;


namespace KatalogKlientow.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetKlients();
        void AddKlients(IEnumerable<Client> klients);
        void AddKlient(Client klient);
        void UpdateKlient(Client klient);
        void DeleteKlient(int id);
    }
}
