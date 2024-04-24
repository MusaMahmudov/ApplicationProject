using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Database.Contexts;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Database.Repositories.Implementations
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
