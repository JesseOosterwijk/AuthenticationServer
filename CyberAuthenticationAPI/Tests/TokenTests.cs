using NUnit.Framework;
using Moq;
using Service.Implementations;
using Service.Interfaces;
using DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System;

namespace Tests
{
    public class TokenTests
    {
        Mock<IEncryptionService> encryptionMock = new Mock<IEncryptionService>();
        JwtTokenService tokenService;
        UserDto testUser;

        [SetUp]
        public void Setup()
        {
            testUser = new UserDto() { Id = "1", Email = "TestMakker@aids.com", Password = "hashedWachtwoord" };
            tokenService = new JwtTokenService(encryptionMock.Object);
        }

        [Test]
        public void GenerateToken_HasCorrectParameters()
        {
            string token = tokenService.GenerateToken(testUser);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken secToken = tokenHandler.ReadJwtToken(token);
            var actualEmail = secToken.Claims.FirstOrDefault(c => c.Value == testUser.Email );
            var actualId = secToken.Claims.FirstOrDefault(c => c.Value == testUser.Id);
            Assert.IsNotNull(actualEmail);
            Assert.IsNotNull(actualId);
        }

        [Test]
        public void VerifyToken_ReturnsTrue_IfTokenIsValid()
        {
            string token = tokenService.GenerateToken(testUser);
            bool result = tokenService.VerifyToken(token);

            Assert.IsTrue(result);
        }

        [Test]
        public void VerifyToken_ReturnsFalse_IfTokenIsInvalid()
        {
            string invalidToken = "InvalidToken";

            Assert.Throws<ArgumentException>(() => tokenService.VerifyToken(invalidToken));
        }
    }
}
