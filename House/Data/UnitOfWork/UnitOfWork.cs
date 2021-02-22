using House.Data.Repository;
using House.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HouseContext _context;
        private IGenericRepository<Location> locationRepository;
        public UnitOfWork(HouseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Location> LocationRepository 
        {
            get
            {
                if (this.locationRepository == null)
                {
                    this.locationRepository = new GenericRepository<Location>(_context);
                }
                return locationRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
