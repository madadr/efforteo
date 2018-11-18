using Efforteo.Common.Commands;
using Efforteo.Common.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Efforteo.Services.Accounts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .Build()
                .Run();
        }
    }
}
