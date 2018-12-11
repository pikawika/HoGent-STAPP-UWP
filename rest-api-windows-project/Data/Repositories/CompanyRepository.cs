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
        private readonly DbSet<Company> _companies;
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
            _merchants = context.Merchants;
            _companies = context.Companies;
        }

        public void addCompany(int userId, Company company)
        {
            _merchants.FirstOrDefault(m => m.UserId == userId)?.Companies.Add(company);
            SaveChanges();
        }

        public Company getById(int companyId)
        {
            return _companies.Include(c => c.Merchant).SingleOrDefault(c => c.CompanyId == companyId && !c.isDeleted);
        }

        public void removeCompany(int companyId)
        {
            Company companyToDelete = _companies.Include(c => c.Establishments).FirstOrDefault(c => c.CompanyId == companyId);
            if (companyToDelete != null)
            {
                companyToDelete.isDeleted = true;
                companyToDelete.Establishments.ForEach(e => e.isDeleted = true);
                companyToDelete.Establishments.ForEach(e => e.Promotions.ForEach(p => p.isDeleted = true));
                companyToDelete.Establishments.ForEach(est => est.Events.ForEach(eve => eve.isDeleted = true));
                SaveChanges();
            }
        }

        public bool isOwnerOfCompany(int userId, int companyId)
        {
            return _companies.Any(c => c.MerchantId == userId && c.CompanyId == companyId && !c.isDeleted);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
