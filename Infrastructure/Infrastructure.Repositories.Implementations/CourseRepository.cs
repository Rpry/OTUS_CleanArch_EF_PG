using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services.Repositories.Abstractions;
using Domain.Entities;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Course;

namespace Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// Репозиторий работы с курсами.
    /// </summary>
    public class CourseRepository: Repository<Course, int>, ICourseRepository
    {
        public CourseRepository(DatabaseContext context): base(context)
        {
        }

        /// <summary>
        /// Получить сущность по ID.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Курс. </returns>
        public override async Task<Course> GetAsync(int id, CancellationToken cancellationToken)
        {
            //var result2 = await Context.Set<Course>().FromSql($"SELECT * from my_proc({id})").SingleOrDefaultAsync(cancellationToken);
            //var result = await Context.Set<Course>().FromSql($"select * from \"Courses\" where \"Id\" = {id}").SingleOrDefaultAsync(cancellationToken);
            //var view = await Context.Set<ViewLesson>().SingleOrDefaultAsync((vl) => vl.Id == id, cancellationToken);
            var query = Context.Set<Course>();
            var result = await query.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

            return result;
        }
        
        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="filterDto"> ДТО фильтра. </param>
        /// <returns> Список курсов. </returns>
        public async Task<List<Course>> GetPagedAsync(CourseFilterDto filterDto)
        {
            var query = GetAll()
                .Include(c => c.Lessons)
                .AsSplitQuery()
                .Where(c => !c.Deleted);

            if (!string.IsNullOrWhiteSpace(filterDto.Name))
            {
                query = query.Where(c => c.Name == filterDto.Name);
            }
            
            if (filterDto.Price.HasValue)
            {
                query = query.Where(c => c.Price == filterDto.Price);
            }

            query = query
                .Skip((filterDto.Page - 1) * filterDto.ItemsPerPage)
                .Take(filterDto.ItemsPerPage);

            return await query.ToListAsync();
        }
        
        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        public async Task<List<CourseInfo>> GetCourseInfosAsync()
        {
            List<CourseInfo> entities = await Context.Set<Course>()
                .Include(c => c.Lessons)
                .Select(c => new CourseInfo
                {
                    Id = c.Id,
                    LessonsCount = c.Lessons.Count,
                })
                .ToListAsync();

            var entities2 = await Context.Set<Course>()
                .Include(c => c.Lessons)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Price
                })
                .ToListAsync();

            foreach (var entity in entities2)
            {
            }
            
            var entities3 = await Context.Set<Course>()
                .Select(c => new
                {
                    Id = c.Id,
                    LessonsCount = Context.Set<Lesson>().Count(l => l.CourseId == c.Id)
                })
                .ToListAsync();
            
            return entities;
        }
    }
}
