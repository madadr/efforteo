using System.Threading.Tasks;

namespace Efforteo.Common.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}