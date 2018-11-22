using System.Collections.Generic;

namespace Efforteo.Common.Auth
{
    // TODO: Add role!
    public class JsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
//        public IDictionary<string, string> Claims { get; set; }
    }
}