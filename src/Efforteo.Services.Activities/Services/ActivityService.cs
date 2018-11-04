using System;
using System.Threading.Tasks;
using Efforteo.Common;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Activities.Domain.Models;
using Efforteo.Services.Activities.Domain.Repositories;

namespace Efforteo.Services.Activities.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ActivityService(IActivityRepository activityRepository, ICategoryRepository categoryRepository)
        {
            _activityRepository = activityRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Guid userId, Guid id, string category, string name, string description,
            DateTime createdAt)
        {
            var categoryObject = await _categoryRepository.GetAsync(category.ToLowerInvariant());

            if (categoryObject == null)
            {
                throw new EfforteoException("invalid_category", $"Category {category} not found");
            }

            await _activityRepository.AddAsync(new Activity(userId, id, name, categoryObject, description,
                createdAt));
        }
    }
}