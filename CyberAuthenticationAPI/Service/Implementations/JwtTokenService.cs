using System;
using DataAccess;

namespace Service
{
    public class JwtTokenService : ITokenService
    {

        public string GenerateToken(UserDto user)
        {
            throw new NotImplementedException();
        }

        public string VerifyToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}