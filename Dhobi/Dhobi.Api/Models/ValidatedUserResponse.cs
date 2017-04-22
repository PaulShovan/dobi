using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhobi.Api.Models
{
    public class ValidatedUserResponse
    {
        public string Name;
        public string Token;
        public ValidatedUserResponse(string name, string token)
        {
            Name = name;
            Token = token;
        }
    }
}