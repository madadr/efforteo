namespace Efforteo.Common.Commands
{
    public class CreateAccount : ICommand
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
