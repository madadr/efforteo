using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Activities.Domain.DTO;
using Efforteo.Services.Activities.Domain.Models;
using Efforteo.Services.Activities.Domain.Repositories;

namespace Efforteo.Services.Activities.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository activityRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(Guid userId, Guid id, string category, string title, string description, long time, float distance)
        {
            var categoryObject = await _categoryRepository.GetAsync(category.ToLowerInvariant());

            if (categoryObject == null)
            {
                throw new EfforteoException("invalid_category", $"Category {category} not found");
            }

            await _activityRepository.AddAsync(new Activity(userId, id, title, categoryObject, description, time, distance));
        }

        public async Task<ActivityDto> GetAsync(Guid id)
        {
            var activity = await _activityRepository.GetAsync(id);
            if (activity == null)
            {
                throw new EfforteoException("activity_not_exists", $"Activity doesn't exist.");
            }

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetAllAsync()
        {
            var activities = await _activityRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(Guid userId)
        {
            var activities = await _activityRepository.GetUserActivitiesAsync(userId);

            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task UpdateAsync(ActivityDto activityDto)
        {
            var activity = await _activityRepository.GetAsync(activityDto.Id);
            if (activity == null)
            {
                throw new EfforteoException("activity_not_exists", $"Activity doesn't exist {activityDto.Id}.");
            }

            if (!string.IsNullOrEmpty(activityDto.Category))
            {
                activityDto.Category = activityDto.Category.ToLowerInvariant();
                var categoryObject = await _categoryRepository.GetAsync(activityDto.Category);

                if (categoryObject == null)
                {
                    throw new EfforteoException("invalid_category", $"Category {activityDto.Category} not found");
                }
            }

            activity.SetData(activityDto.Title, activityDto.Category, activityDto.Description, activityDto.Time, activityDto.Distance);

            await _activityRepository.UpdateAsync(activity);
        }

        public async Task RemoveAsync(Guid id)
        {
            var activity = await _activityRepository.GetAsync(id);
            if (activity == null)
            {
                throw new EfforteoException("activity_not_exists", $"Activity doesn't exist.");
            }

            await _activityRepository.RemoveAsync(id);
        }
    }
}