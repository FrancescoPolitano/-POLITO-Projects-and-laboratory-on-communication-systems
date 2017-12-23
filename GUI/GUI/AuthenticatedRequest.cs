using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class AuthenticatedRequest
    {
        private string token;
        private object body;

        public AuthenticatedRequest()
        {
        }

        public AuthenticatedRequest(string token, object body)
        {
            Token = token;
            Body = body;
        }

        public object Body { get => body; set => body = value; }
        public string Token { get => token; set => token = value; }
    }
}
