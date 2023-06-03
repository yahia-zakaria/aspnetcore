using Entities;
using ServiceContracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CountryRepository : RepositoryBase<Country>
    {
        private readonly ApplicationDbContext context;
        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void x()
        {
            throw new NotImplementedException();
        }
    }
}
