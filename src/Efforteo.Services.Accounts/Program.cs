using Efforteo.Common.Events;
using Efforteo.Common.Services;

namespace Efforteo.Services.Accounts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .Subscribe<UserCreated>()
                .Subscribe<UserAuthenticated>()
                .Subscribe<UserRemoved>()
                .Build()
                .Run();
        }
    }
}