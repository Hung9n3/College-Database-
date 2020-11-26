using Contracts;
using Entity.Context;
using Entity.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RepoAddress : RepoBase<Address>, IRepoAddress
    {
        public RepoAddress(RepoContext repoContext) : base (repoContext)
        {

        }
    }
}
