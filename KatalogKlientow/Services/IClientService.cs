using KatalogKlientow.Models;
using System.Collections.Generic;

namespace KatalogKlientow.Services
{
    public interface IClientService
    {
        List<Client> GetAllClients();
        List<Client> AddOrUpdateClient(Client klient);
        List<Client> DeleteClient(int id);
    }
}
