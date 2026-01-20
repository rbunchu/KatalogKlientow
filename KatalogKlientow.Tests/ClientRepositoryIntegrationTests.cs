using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using KatalogKlientow.Models;
using KatalogKlientow.Repositories;
using KatalogKlientow.Configuration;
using System.Linq;
using System.Collections.Generic;
using KatalogKlientow.Infrastructure;

namespace KatalogKlientow.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class ClientRepositoryIntegrationTests
    {
        private ServiceProvider _sp;
        private IClientRepository _repo;
        private DatabaseCleaner _dbCleaner;

        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddAppServices();            
            _sp = services.BuildServiceProvider();

            _repo = _sp.GetRequiredService<IClientRepository>();

            var databaseInitializer = _sp.GetRequiredService<Infrastructure.DatabaseInitializer>();
            _dbCleaner = _sp.GetRequiredService<DatabaseCleaner>();
            databaseInitializer.Initialize();

            _dbCleaner.CleanData();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _dbCleaner.CleanData();
            }
            catch { }
            _sp?.Dispose();
        }

        [Test]
        public void CleanData_RemovesAllRows()
        {
            var c = new Client { Nazwa = "X", Nip = "0000000001", Email = "x@test.local" };
            _repo.AddKlient(c);

            _dbCleaner.CleanData();
            var all = _repo.GetKlients().ToList();

            Assert.IsEmpty(all);
        }

        [Test]
        public void AddKlient_AddsSingleRow()
        {
            _dbCleaner.CleanData();
            var c = new Client { Nazwa = "Single", Nip = "0000000002", Email = "single@test.local", Adres = "Addr" };

            _repo.AddKlient(c);
            var all = _repo.GetKlients().ToList();

            Assert.IsNotEmpty(all);
            Assert.IsTrue(all.Any(x => x.Nazwa == "Single" && x.Nip == "0000000002"));
        }

        [Test]
        public void AddKlients_AddsMultipleRows()
        {
            _dbCleaner.CleanData();
            var list = new List<Client>
            {
                new Client { Nazwa = "A", Nip = "0000000003", Email = "a@test.local" },
                new Client { Nazwa = "B", Nip = "0000000004", Email = "b@test.local" }
            };

            _repo.AddKlients(list);
            var all = _repo.GetKlients().ToList();

            Assert.GreaterOrEqual(all.Count, 2);
            Assert.IsTrue(all.Any(x => x.Nazwa == "A"));
            Assert.IsTrue(all.Any(x => x.Nazwa == "B"));
        }

        [Test]
        public void UpdateKlient_UpdatesRow()
        {
            _dbCleaner.CleanData();
            var list = new List<Client>
            {
                new Client { Nazwa = "ToUpdate", Nip = "0000000005", Email = "up@test.local" }
            };
            _repo.AddKlients(list);

            var inserted = _repo.GetKlients().FirstOrDefault();
            Assert.IsNotNull(inserted, "Inserted client not found");
            var newName = "UpdatedName";
            inserted.Nazwa = newName;

            _repo.UpdateKlient(inserted);
            var reloaded = _repo.GetKlients().FirstOrDefault(k => k.Id == inserted.Id);

            Assert.IsNotNull(reloaded);
            Assert.AreEqual(newName, reloaded.Nazwa);
        }

        [Test]
        public void DeleteKlient_RemovesRow()
        {
            _dbCleaner.CleanData();
            var list = new List<Client>
            {
                new Client { Nazwa = "Keep", Nip = "0000000006", Email = "keep@test.local" },
                new Client { Nazwa = "RemoveMe", Nip = "0000000007", Email = "rm@test.local" }
            };
            _repo.AddKlients(list);

            var allBefore = _repo.GetKlients().ToList();
            Assert.GreaterOrEqual(allBefore.Count, 2);

            var toDelete = allBefore.FirstOrDefault(c => c.Nazwa == "RemoveMe");
            Assert.IsNotNull(toDelete, "Client to delete not found");

            _repo.DeleteKlient(toDelete.Id);
            var allAfter = _repo.GetKlients().ToList();

            Assert.IsFalse(allAfter.Any(c => c.Id == toDelete.Id));
            Assert.IsTrue(allAfter.Any(c => c.Nazwa == "Keep"));
        }
    }
}