using NUnit.Framework;
using KatalogKlientow.Services;
using KatalogKlientow.Models;
using System.Linq;

namespace KatalogKlientow.Tests
{
    [TestFixture]
    public class ModelValidatorTests
    {
        [Test]
        public void Validate_ValidKlient_NoErrors()
        {
            var validator = new ModelValidator();
            var klient = new Client
            {
                Nazwa = "Firma Testowa",
                Nip = "0123456789",
                Email = "kontakt@firma.test",
                Adres = "Ulica 1",
                NrTel = "123456789"
            };

            var errors = validator.Validate(klient);

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void Validate_MissingRequiredFields_ReturnsValidationResults()
        {
            var validator = new ModelValidator();
            var klient = new Client
            {
                Nip = "123",
                Adres = "Ulica 2"
            };

            var errors = validator.Validate(klient);

            Assert.IsNotNull(errors);
            Assert.IsTrue(errors.Any(), "Expected validation errors but found none.");

            Assert.IsTrue(errors.Any(e => e.MemberNames.Any(m => m == "Nazwa")), "Expected error for 'Nazwa'");
            Assert.IsTrue(errors.Any(e => e.MemberNames.Any(m => m == "Email")), "Expected error for 'Email'");
            Assert.IsTrue(errors.Any(e => e.MemberNames.Any(m => m == "Nip")), "Expected error for 'Nip'");
        }
    }
}