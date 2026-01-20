using KatalogKlientow.Models;
using KatalogKlientow.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace KatalogKlientow.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _klientRepository;
        public ClientService(IClientRepository klientRepository)
        {
            _klientRepository = klientRepository;
        }

        public List<Client> AddOrUpdateClient(Client klient)
        {
            if (klient.Id == 0)
            {
                _klientRepository.AddKlient(klient);
            }
            else
            {
                _klientRepository.UpdateKlient(klient);
            }

            return GetAllClients();
        }

        public List<Client> DeleteClient(int klientId)
        {
            _klientRepository.DeleteKlient(klientId);
            return GetAllClients();
        }

        public List<Client> GetAllClients()
        {
            return _klientRepository.GetKlients().ToList();
        }
    }
}
