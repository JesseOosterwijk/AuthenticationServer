using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberAuthenticationAPI.Requests
{
    public class UpdateRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
