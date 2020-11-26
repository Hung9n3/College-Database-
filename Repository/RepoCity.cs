using Contracts;
using Entity.Context;
using Entity.Location;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RepoCity : RepoBase<City>, IRepoCity
    {
        public RepoCity(RepoContext repoContext) : base(repoContext)
        {

        }
    }
}
