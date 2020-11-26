using Contracts;
using Entity.Context;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RepoStudent : RepoBase<Student>,IRepoStudent
    {
        public RepoStudent(RepoContext repoContext): base (repoContext)
        {

        }
    }
}
