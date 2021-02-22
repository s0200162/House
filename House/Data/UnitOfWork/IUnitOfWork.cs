using House.Data.Repository;
using House.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Location> LocationRepository { get; }
        Task SaveAsync();
    }
}
