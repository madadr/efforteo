using System;

namespace Efforteo.Common.Commands
{
    public class CreateUser : ICommand
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
