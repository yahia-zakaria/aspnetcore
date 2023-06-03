using Entities;
using ServiceContracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class PersonRepository : RepositoryBase<Person>
    {
        private readonly ApplicationDbContext context;
        public PersonRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
