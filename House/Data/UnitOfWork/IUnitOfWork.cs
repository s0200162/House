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
        IGenericRepository<Room> RoomRepository { get; }
        IGenericRepository<Profession> ProfessionRepository { get;  }
        IGenericRepository<Invoice> InvoiceRepository { get; }
        IGenericRepository<Reservation> ReservationRepository { get; }
        Task SaveAsync();
    }
}
