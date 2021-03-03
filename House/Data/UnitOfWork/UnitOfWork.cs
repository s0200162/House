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
        private IGenericRepository<Room> roomRepository;
        private IGenericRepository<Profession> professionRepository;
        private IGenericRepository<Invoice> invoiceRepository;
        private IGenericRepository<Reservation> reservationRepository;
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

        public IGenericRepository<Room> RoomRepository
        {
            get
            {
                if(this.roomRepository == null)
                {
                    this.roomRepository = new GenericRepository<Room>(_context);
                }
                return roomRepository;
            }
        }

        public IGenericRepository<Profession> ProfessionRepository
        {
            get
            {
                if (this.professionRepository == null)
                {
                    this.professionRepository = new GenericRepository<Profession>(_context);
                }
                return professionRepository;
            }
        }

        public IGenericRepository<Invoice> InvoiceRepository
        {
            get
            {
                if (this.invoiceRepository == null)
                {
                    this.invoiceRepository = new GenericRepository<Invoice>(_context);
                }
                return invoiceRepository;
            }
        }

        public IGenericRepository<Reservation> ReservationRepository
        {
            get
            {
                if (this.reservationRepository == null)
                {
                    this.reservationRepository = new GenericRepository<Reservation>(_context);
                }
                return reservationRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
