using NUnit.Framework;
using Moq;
using Service.Implementations;
using Service.Interfaces;
using DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.Win32;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Tests
{
    //TODO: Add working test with integrated public/private key pair mocks
    public class TokenTests
    {
        Mock<IEncryptionService> encryptionMock = new Mock<IEncryptionService>();
        Mock<IKeypairRepository> keypairMock = new Mock<IKeypairRepository>();
        Mock<JwtTokenService> tokenMock = new Mock<JwtTokenService>();
        JwtTokenService tokenService;
        UserDto testUser;
        KeypairDto testKeyPair;

        [SetUp]
        public void Setup()
        {
            testUser = new UserDto() { Id = "1", Email = "TestMakker@aids.com", Password = "hashedWachtwoord" };
            testKeyPair = new KeypairDto("1", "<RSAKeyValue><Modulus>0Z/2DfZjhyTobvQQsxhFEaf3UQkYsGtFjPjiAnZqHnDEdaw7iZIWPCroh3Mh4o0wQvMaxiDdCFCSITh0Itt7WdHJpAt3Q0NuZxY7k9TYdjuSKh5H36il26MOMfly2OTSEK6tupm/9LYY69+5tLGlA9WYw75p8EJuHeOVWiqD6Ezg3grC5d7pBEAB+BGyblB+RKS0+DmFaYQ3qTb/JT6IoFm+2E2FioiP2SwenHmFe41tIdvqUm1P8qWY6u23S/99TB+dSUkQm2DAi3veVIF18EBBw5HjI3lgPzYKelw9Ni6g6oPalS0J7goMbHGEis3GYWZPrny4d+XSV3WSuT0u6Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
                "<RSAKeyValue><Modulus>0Z/2DfZjhyTobvQQsxhFEaf3UQkYsGtFjPjiAnZqHnDEdaw7iZIWPCroh3Mh4o0wQvMaxiDdCFCSITh0Itt7WdHJpAt3Q0NuZxY7k9TYdjuSKh5H36il26MOMfly2OTSEK6tupm/9LYY69+5tLGlA9WYw75p8EJuHeOVWiqD6Ezg3grC5d7pBEAB+BGyblB+RKS0+DmFaYQ3qTb/JT6IoFm+2E2FioiP2SwenHmFe41tIdvqUm1P8qWY6u23S/99TB+dSUkQm2DAi3veVIF18EBBw5HjI3lgPzYKelw9Ni6g6oPalS0J7goMbHGEis3GYWZPrny4d+XSV3WSuT0u6Q==</Modulus><Exponent>AQAB</Exponent><P>2DTJwGY0BWXnyk2gGydcFvfhKMKdmjyvhrpDBGZNIFuOhrLvDl21zt9fG1ozt6Wbn3/kw1EYBX3V84MvKOREC24Sa3b02GWO9fb0MqILxxmcOBhDLWf1s7e76WRxA/zWyQCE5hdiNx852+5JIYICcL6xglKu/SbWDjZloIX6kQc=</P><Q>+DUSkJE+04/zyFH+ivstV5fTr2GG6A8knkR4TVYAQvUuhiTZ7M6aseHZ47gUfT95c7uxfPZ3gh8n61qvaEyYGp4WIkIkDgBkAqGsAhYHb4gxmxI+Pk+Qo24LB3PKq6qEEC/DBazcSrqa5EaakLZrzlA+zA4xB+8ApGeACgI5dI8=</Q><DP>ZSg1qQ5okon+bDnriijdPmXV9CMyaAKywV+OkZVnypbr1XwMu3T+5n6+WebXQbp7WKIXH4dmrNMWcmvTeddrOsnSVnN+1WyW+eAEm1gUGKCT5e78J1d3rxFtGyMCebInsD3M8HbKXi3+/Ta9Aq94gtXF6crT0uAD0PyEihmFhas=</DP><DQ>D07mqDftEgtM08wK0POQ5lgc3DI9qc5VSdWbPdnSBk8s4WUgc7SCYfo2AA94ZAkoKnUnCzgAQuw9AA0FyH8A2pNpdg9yPdLUWD00aKSHLN/Pf2run9U0bH/6+iiwFUpc/sqTUqRIqes8ZlvcaJR3ra/RT7CIYZ7iAUHI6dUdSg0=</DQ><InverseQ>Xpjv7GTPef3hIGP+xQu80rFqtT0ZaiSB+ueoM7jN6N26yrJtbuf3Or9Z+gj0YEXs3GJM6Kt9e9ThNwU2soL5Ib4wcm1dmttqgXq3Axrwep+xtUmcDJfKBONFw7YEwlTUF4VLbdvAHncGa0oeIYBxPo6mj31BgYM9QXA4ZKorRhk=</InverseQ><D>s6Acmi5NbtrDiX/+Z1kepliF5pyi2VmyiXAjSvTpThhXYHYrtmDGSNyD0L1phdZySoxCnxA+FwfjkC/t5IiqlHbeEIH6ulqn2hIFD1JOAlJdl1XHRPIg3apFWbFlJlI9ZyonmERzZmA7zrMxUJMA0d5QPzuFT+m8rkP8EP1pdr4L5WJ/IkzbHVoWx3YeTTjQTr1TPzCaMPBVrly4ZbptvGCLb0Ijx0TGOkGM/z/uzGFot5dudE1Dg5JsLRCUGKEZ/KfaYDXFlW7y7TophvA5syD/gI3Jp2jknTTEhFbctkznUsBUjhg35wkcvGX8mbVeId0YlMb4N8i56KpVzt8I2Q==</D></RSAKeyValue>");
            tokenService = new JwtTokenService(encryptionMock.Object, keypairMock.Object);
            keypairMock.Setup(x => x.GetKeypair("1"))
                .Returns(Task.Run(() => testKeyPair));
        }

        [Test]
        public async Task GenerateToken_HasCorrectParameters()
        {
            string token = await tokenService.GenerateToken(testUser, testKeyPair.PrivateKey);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken secToken = tokenHandler.ReadJwtToken(token);
            var actualEmail = secToken.Claims.FirstOrDefault(c => c.Value == testUser.Email );
            var actualId = secToken.Claims.FirstOrDefault(c => c.Value == testUser.Id);
            Assert.IsNotNull(actualEmail);
            Assert.IsNotNull(actualId);
        }

        [Test]
        public async Task VerifyToken_ReturnsTrue_IfTokenIsValid()
        {
            string token = await tokenService.GenerateToken(testUser, testKeyPair.PrivateKey);
            bool result = await tokenService.VerifyToken(token);

            Assert.IsTrue(result);
        }

        [Test]
        public void VerifyToken_ReturnsFalse_IfTokenIsInvalid()
        {
            string invalidToken = "InvalidToken";

            Assert.ThrowsAsync<ArgumentException>(async () => await tokenService.VerifyToken(invalidToken));
        }
    }
}
