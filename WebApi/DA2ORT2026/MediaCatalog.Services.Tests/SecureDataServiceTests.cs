
using MediaCatalog.Services.Settings;
using Microsoft.Extensions.Options;
using Moq;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class SecureDataServiceTests
    {
        private SecureDataService _secureDataService;
        private string _plainTextString;
        private string _password;

        [TestInitialize]
        public void Setup()
        {
            var keyBytes = new byte[32];
            new Random().NextBytes(keyBytes);
            var base64Key = Convert.ToBase64String(keyBytes);

            var settings = new SystemSettings
            {
                EncryptionKey = base64Key
            };

            var mockOptions = new Mock<IOptions<SystemSettings>>();
            mockOptions.Setup(o => o.Value).Returns(settings);

            _secureDataService = new SecureDataService(mockOptions.Object);

            _plainTextString = "to be encrypted";
            _password = "pa$$w0rd.123";
        }

        [TestMethod]
        public void Encrypt_WhenCalledWithPlainText_ThenEncryptedStringIsReturned()
        {
            //arrange
            //act
            string encryptedString = _secureDataService.Encrypt(_plainTextString);

            //assert
            Assert.AreNotEqual(_plainTextString, encryptedString);
        }

        [TestMethod]
        public void Decrypt_WhenCalled_ThenDecryptedStringMatchPlainText()
        {
            //arrange
            string encryptedString = _secureDataService.Encrypt(_plainTextString);

            //act
            string decryptedString = _secureDataService.Decrypt(encryptedString);

            //assert
            Assert.AreEqual(_plainTextString, decryptedString);
        }

        [TestMethod]
        public void Hash_WhenCalledWithPlainText_ThenHashedValueIsReturned()
        {
            //arrange
            //act
            string hash = _secureDataService.Hash(_plainTextString);

            // Assert
            Assert.AreNotEqual(_plainTextString, hash);
        }

        [TestMethod]
        public void CompareHashes_WhenCalled_ThenTrueIsReturned()
        {
            //arrange
            //act
            string hash = _secureDataService.Hash(_password);
            bool result = _secureDataService.CompareHashes(hash, _password);

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CompareHashes_WhenCalledWithWrongData_ThenFalseIsReturned()
        {
            //arrange
            string wrongPassword = "password1234";
            string hash = _secureDataService.Hash(_password);

            //act
            bool result = _secureDataService.CompareHashes(hash, wrongPassword);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Hash_WhenCalledWithTheSameData_ThenDifferentHashValuesAreReturned()
        {
            //arrange
            //act
            string hash1 = _secureDataService.Hash(_password);
            string hash2 = _secureDataService.Hash(_password);

            //assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
