using Contracts;
using Entity.Context;
using Entity.Location;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RepoDistrict : RepoBase<Districts>, IRepoDistrict
    {
        public RepoDistrict(RepoContext repoContext) : base(repoContext)
        {

        }
    }
}