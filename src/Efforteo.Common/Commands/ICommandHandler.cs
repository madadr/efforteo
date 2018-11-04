using System.Threading.Tasks;

namespace Efforteo.Common.Commands
{
    public interface ICommandHandler <in T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}
