using Moq;
using NUnit.Framework;
using KatalogKlientow.Repositories;
using KatalogKlientow.Services;
using KatalogKlientow.Models;
using System.Collections.Generic;
using System.Linq;

namespace KatalogKlientow.Tests
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _repoMock;
        private ClientService _service;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IClientRepository>();
            _service = new ClientService(_repoMock.Object);
        }

        [Test]
        public void GetAllClients_ReturnsListFromRepository()
        {
            var expected = new List<Client>
            {
                new Client { Id = 1, Nazwa = "A", Nip = "0000000001", Email = "a@x.com" },
                new Client { Id = 2, Nazwa = "B", Nip = "0000000002", Email = "b@x.com" }
            };

            _repoMock.Setup(r => r.GetKlients()).Returns(expected);

            var result = _service.GetAllClients();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.Id == 1 && c.Nazwa == "A"));
            _repoMock.Verify(r => r.GetKlients(), Times.Once);
        }

        [Test]
        public void AddOrUpdateClient_WhenIdIsZero_CallsAddKlientAndReturnsUpdatedList()
        {
            var newClient = new Client { Id = 0, Nazwa = "New", Nip = "0000000003", Email = "new@x.com" };
            var afterInsert = new List<Client>
            {
                new Client { Id = 3, Nazwa = "New", Nip = "0000000003", Email = "new@x.com" }
            };

            _repoMock.Setup(r => r.AddKlient(It.Is<Client>(c => c.Nazwa == "New" && c.Nip == "0000000003")));
            _repoMock.Setup(r => r.GetKlients()).Returns(afterInsert);

            var result = _service.AddOrUpdateClient(newClient);

            _repoMock.Verify(r => r.AddKlient(It.IsAny<Client>()), Times.Once);
            _repoMock.Verify(r => r.UpdateKlient(It.IsAny<Client>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result[0].Id);
        }

        [Test]
        public void AddOrUpdateClient_WhenIdIsNotZero_CallsUpdateKlientAndReturnsUpdatedList()
        {
            var existing = new Client { Id = 5, Nazwa = "Exists", Nip = "0000000005", Email = "e@x.com" };
            var afterUpdate = new List<Client>
            {
                new Client { Id = 5, Nazwa = "Exists modified", Nip = "0000000005", Email = "e@x.com" }
            };

            _repoMock.Setup(r => r.UpdateKlient(It.Is<Client>(c => c.Id == 5)));
            _repoMock.Setup(r => r.GetKlients()).Returns(afterUpdate);

            var result = _service.AddOrUpdateClient(existing);

            _repoMock.Verify(r => r.UpdateKlient(It.IsAny<Client>()), Times.Once);
            _repoMock.Verify(r => r.AddKlient(It.IsAny<Client>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[0].Id);
            Assert.AreEqual("Exists modified", result[0].Nazwa);
        }

        [Test]
        public void DeleteClient_CallsRepositoryDeleteAndReturnsUpdatedList()
        {
            var afterDelete = new List<Client>
            {
                new Client { Id = 10, Nazwa = "Remaining", Nip = "0000000010", Email = "r@x.com" }
            };

            _repoMock.Setup(r => r.DeleteKlient(7));
            _repoMock.Setup(r => r.GetKlients()).Returns(afterDelete);

            var result = _service.DeleteClient(7);

            _repoMock.Verify(r => r.DeleteKlient(7), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(10, result[0].Id);
        }
    }
}