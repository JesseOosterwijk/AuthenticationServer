using NUnit.Framework;
using Moq;
using DataAccess.Interfaces;
using DataAccess;
using Service.Implementations;
using Service.Interfaces;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace Tests
{
    public class Tests
    {
        Mock<IUserRepository> userMock = new Mock<IUserRepository>();
        Mock<IHashService> hashMock = new Mock<IHashService>();
        Mock<ISaltRepository> saltMock = new Mock<ISaltRepository>();
        Mock<ITokenService> tokenMock = new Mock<ITokenService>();
        UserDto testUser;
        UserService userService;
        byte[] salt;

        [SetUp]
        public void Setup()
        {
            userService = new UserService(hashMock.Object, userMock.Object, tokenMock.Object, saltMock.Object);
            salt = new byte[32];
            testUser = new UserDto() { Id = "1", Email = "TestMakker@aids.com", Password = "hashedWachtwoord" };
            userMock.Setup(x => x.GetUser("TestMakker@aids.com"))
                    .Returns(Task.Run(() => testUser));

            userMock.Setup(x => x.GetUser("1"))
                    .Returns(Task.Run(() => testUser));

            saltMock.Setup(x => x.GetSalt("1"))
                    .Returns(Task.Run(() => new byte[1]));

            hashMock.Setup(x => x.HashAsync("VeiligWachtwoord", new byte[1]))
                    .Returns(Task.Run(() => "hashedWachtwoord"));

            hashMock.Setup(x => x.HashAsync("OnveiligWachtwoord", new byte[1]))
                    .Returns(Task.Run(() => "foutwachtwoord"));

            hashMock.Setup(x => x.HashAsync("VeiligWachtwoord", out salt))
                    .Returns(Task.Run(() => "hashedWachtwoord"));

            tokenMock.Setup(x => x.GenerateToken(testUser))
                     .Returns("poggers");
        }

        [Test]
        public async Task CorrectCredentials_LoginTest()
        {
            var expected = "poggers";
            var actual = userService.Login("TestMakker@aids.com", "VeiligWachtwoord");

            Assert.AreEqual(await actual, expected);
        }

        [Test]
        public void IncorrectCredentials_LoginTest()
        {
            Assert.ThrowsAsync(typeof(AuthenticationException), async () => await userService.Login("TestMakker@aids.com", "OnveiligWachtwoord"));
        }

        [Test]
        public async Task RegisterUserTest()
        {
            await userService.AddUser("TestMakker@aids.com", "VeiligWachtwoord");
            userMock.Verify(x => x.InsertUser(It.IsAny<string>(), "TestMakker@aids.com", "hashedWachtwoord"), Times.Once());
            saltMock.Verify(x => x.InsertSalt(It.IsAny<string>(), salt));
        }

        [Test]
        public async Task DeleteUserTest_Correct()
        {
            await userService.DeleteUser("1", "VeiligWachtwoord");
            userMock.Verify(x => x.DeleteUser("1"), Times.Once());
            saltMock.Verify(x => x.DeleteSalt("1"), Times.Once());
        }

        [Test]
        public void DeleteUserTest_Incorrect()
        {
            Assert.ThrowsAsync(typeof(AuthenticationException), async () => await userService.DeleteUser("1", "OnveiligWachtwoord"));
        }
    }
}