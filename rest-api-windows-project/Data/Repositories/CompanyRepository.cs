using System;
using System.Collections.Generic;
using System.Linq;
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
            Company company = _companies
                .Include(c => c.Merchant)
                .Include(c => c.Establishments).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(c => c.Establishments).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(c => c.Establishments).ThenInclude(e => e.Images)
                .Include(c => c.Establishments).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(c => c.Establishments).ThenInclude(e => e.ExceptionalDays)
                .Include(c => c.Establishments).ThenInclude(e => e.Promotions).ThenInclude(od => od.Images)
                .Include(c => c.Establishments).ThenInclude(e => e.Promotions).ThenInclude(od => od.Attachments)
                .Include(c => c.Establishments).ThenInclude(e => e.Events).ThenInclude(od => od.Images)
                .Include(c => c.Establishments).ThenInclude(e => e.Events).ThenInclude(od => od.Attachments)
                .SingleOrDefault(c => c.CompanyId == companyId && !c.isDeleted);

            if (company?.Establishments == null) return company;
            {
                company.Establishments.RemoveAll(e => e.isDeleted);
                foreach (Establishment establishment in company.Establishments)
                {
                    establishment.Promotions.RemoveAll(p => p.EndDate < DateTime.Today || p.isDeleted);
                    establishment.Events.RemoveAll(e => e.EndDate < DateTime.Today || e.isDeleted);
                }
            }

            return company;

        }

        public List<Company> getFromMerchant(int merchantId)
        {
            List<Company> companies = _companies
                .Where(c => c.MerchantId == merchantId && !c.isDeleted)
                .Include(c => c.Merchant)
                .Include(c => c.Establishments).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(c => c.Establishments).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(c => c.Establishments).ThenInclude(e => e.Images)
                .Include(c => c.Establishments).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(c => c.Establishments).ThenInclude(e => e.ExceptionalDays)
                .Include(c => c.Establishments).ThenInclude(e => e.Promotions).ThenInclude(od => od.Images)
                .Include(c => c.Establishments).ThenInclude(e => e.Promotions).ThenInclude(od => od.Attachments)
                .Include(c => c.Establishments).ThenInclude(e => e.Events).ThenInclude(od => od.Images)
                .Include(c => c.Establishments).ThenInclude(e => e.Events).ThenInclude(od => od.Attachments)
                .ToList();



            foreach (Company company in companies)
            {
                company.Establishments.RemoveAll(e => e.isDeleted);
                if (company.Establishments != null)
                {
                    foreach (Establishment establishment in company.Establishments)
                    {
                        establishment.Promotions.RemoveAll(p => p.EndDate < DateTime.Today || p.isDeleted);
                        establishment.Events.RemoveAll(e => e.EndDate < DateTime.Today || e.isDeleted);
                    }
                }
            }


            return companies;


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
