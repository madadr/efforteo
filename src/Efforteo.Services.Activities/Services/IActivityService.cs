using System;
using System.Threading.Tasks;

namespace Efforteo.Services.Activities.Services
{
    public interface IActivityService
    {
        Task AddAsync(Guid userId, Guid id, string category, string name, string description, DateTime createdAt);
    }
}