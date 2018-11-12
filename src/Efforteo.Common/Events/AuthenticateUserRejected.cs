namespace Efforteo.Common.Events
{
    public class AuthenticateUserRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Name { get; }
        public string Code { get; }
        public string Reason { get; }

        protected AuthenticateUserRejected()
        {
        }

        public AuthenticateUserRejected(string email, string name, string code, string reason)
        {
            Email = email;
            Name = name;
            Code = code;
            Reason = reason;
        }
    }
}
