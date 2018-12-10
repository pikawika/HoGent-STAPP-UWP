using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DbSet<Merchant> _merchants;
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
            _merchants = context.Merchants;
        }

        public void addCompany(int userId, Company company)
        {
            _merchants.FirstOrDefault(m => m.UserId == userId)?.Companies.Add(company);
            SaveChanges();
        }

        public Company getById(int userId, int companyId)
        {
            return _merchants.Include(m => m.Companies).FirstOrDefault(m => m.UserId == userId)?.Companies.FirstOrDefault(c => c.CompanyId == companyId);
        }

        public void removeCompany(int userId, int companyId)
        {
            Company companyToDelete = _merchants.Include(m => m.Companies).FirstOrDefault(m => m.UserId == userId)?.Companies.FirstOrDefault(c => c.CompanyId == companyId);
            if (companyToDelete != null)
                companyToDelete.isDeleted = true;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
