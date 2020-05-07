using DataAccess;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        Task<(string, DateTime)> GenerateToken(UserDto user, string _privKey);
        Task<bool> VerifyToken(string token);
        string GenerateRefreshString();
        RsaSecurityKey BuildRsaSigningKey(string xmlParams);
    }
}