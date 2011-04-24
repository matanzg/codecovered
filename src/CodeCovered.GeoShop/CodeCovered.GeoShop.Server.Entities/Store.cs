using System;
using CodeCovered.GeoShop.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using CodeCovered.GeoShop.Infrastructure.Entities;

namespace CodeCovered.GeoShop.Entities
{
    public class Store : IntEntity
    {
        public Store()
        {
            _branches = new HashSet<Branch>();
        }

        public virtual string Name { get; set; }
        public virtual Person Contact { get; set; }

        private ICollection<Branch> _branches;

        public virtual IEnumerable<Branch> Branches
        {
            get { return _branches.AsEnumerable(); }
        }
        public virtual void AddBranch(Branch branch)
        {
            Validate.ThatArgumentNotNull(() => branch);

            if (!_branches.Contains(branch))
            {
                _branches.Add(branch);
                branch.AssignStore(this);
            }
        }
    }
}