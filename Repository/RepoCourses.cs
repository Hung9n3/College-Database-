using Contracts;
using Entity.Context;
using Entity.Course;
using Entity.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepoCourses : RepoBase<Courses>, IRepoCourses
    {
        public RepoCourses(RepoContext repoContext) : base(repoContext)
        {
        }
        public override async Task<List<Courses>> FindAll()
        {
            var items = await _repoContext.Courses.Include(x => x.StudentCourses).Include(x => x.Teacher)
                                            .Include(x => x.Department).AsNoTracking().ToListAsync();
            return items;
        }
    }
}
