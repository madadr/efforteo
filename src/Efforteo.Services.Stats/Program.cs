using Efforteo.Common.Events;
using Efforteo.Common.Services;

namespace Efforteo.Services.Stats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .Subscribe<ActivityCreated>()
                .Subscribe<ActivityUpdated>()
                .Subscribe<ActivityRemoved>()
                .Build()
                .Run();
        }
    }
}