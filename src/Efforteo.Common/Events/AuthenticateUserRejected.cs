namespace Efforteo.Common.Events
{
    public class AuthenticateUserRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Name { get; }
        public string Reason { get; }
        public string Code { get; }

        protected AuthenticateUserRejected()
        {
        }

        public AuthenticateUserRejected(string email, string name, string reason, string code)
        {
            Email = email;
            Name = name;
            Reason = reason;
            Code = code;
        }
    }
}
