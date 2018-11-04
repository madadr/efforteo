using System.Threading.Tasks;

namespace Efforteo.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}