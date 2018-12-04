using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.Domain;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbSet<Customer> _customers;
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
            _customers = context.Customers;
        }

        public void addSubscription(int userId, EstablishmentSubscription establishmentSubscription)
        {
            _customers.FirstOrDefault(c => c.UserId == userId).EstablishmentSubscriptions.Add(establishmentSubscription);
            SaveChanges();
        }

        public void removeSubscription(int userId, EstablishmentSubscription establishmentSubscription)
        {
            _customers.FirstOrDefault(c => c.UserId == userId).EstablishmentSubscriptions.Remove(establishmentSubscription);
            SaveChanges();
        }

        public Customer getById(int userId)
        {
            return _customers.Include(c => c.EstablishmentSubscriptions).FirstOrDefault(u => u.UserId == userId);
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
