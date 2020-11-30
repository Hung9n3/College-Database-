﻿using Contracts;
using Entity.Context;
using Entity.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class RepoTeacher : RepoBase<Teacher>, IRepoTeacher
    {
        public RepoTeacher(RepoContext repoContext) : base(repoContext)
        {

        }
        public override async Task<Teacher> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var item = await _repoContext.Teachers.Include(x => x.Courses).FirstOrDefaultAsync(x => x.TeacherId == id);
            return item;
        }
        public override Task<List<Teacher>> FindAll()
        {
            return base.FindAll();
        }
    }
}