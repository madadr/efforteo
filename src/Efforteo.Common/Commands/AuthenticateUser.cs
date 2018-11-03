using System;
using System.Collections.Generic;
using System.Text;

namespace Efforteo.Common.Commands
{
    public class AuthenticateUser : ICommand
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
