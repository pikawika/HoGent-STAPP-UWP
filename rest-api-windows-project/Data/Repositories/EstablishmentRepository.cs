using System;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace stappBackend.Data.Repositories
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly DbSet<Company> _companies;
        private readonly DbSet<Establishment> _establishments;
        private readonly ApplicationDbContext _context;

        public EstablishmentRepository(ApplicationDbContext context)
        {
            _context = context;
            _companies = context.Companies;
            _establishments = context.Establishments;
        }

        public IEnumerable<Establishment> GetAll()
        {
            var establishments = _establishments
                .Where(e => !e.isDeleted)
                .OrderBy(e => (e.Promotions.Count() + e.Events.Count()))
                .Include(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Images)
                .Include(e => e.ExceptionalDays)
                .Include(e => e.Promotions).ThenInclude(p => p.Images)
                .Include(e => e.Events).ThenInclude(e => e.Images)
                .ToList();

            if (establishments != null)
            {
                foreach (Establishment establishment in establishments)
                {
                    establishment.Promotions.RemoveAll(p => p.EndDate < DateTime.Now || p.isDeleted);
                    establishment.Events.RemoveAll(e => e.EndDate < DateTime.Now || e.isDeleted);
                }
            }

            return establishments;
        }

        public Establishment getById(int id)
        {
            var establishment = _establishments.Where(e => e.EstablishmentId == id && !e.isDeleted)
                .Include(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Images)
                .Include(e => e.ExceptionalDays)
                .Include(e => e.Promotions).ThenInclude(p => p.Images)
                .Include(e => e.Events).ThenInclude(e => e.Images)
                .Include(e => e.Promotions).ThenInclude(p => p.Attachments)
                .Include(e => e.Events).ThenInclude(e => e.Attachments)
                .FirstOrDefault();

            if (establishment != null)
            {
                establishment.Promotions.RemoveAll(p => p.EndDate < DateTime.Now || p.isDeleted);
                establishment.Events.RemoveAll(e => e.EndDate < DateTime.Now || e.isDeleted);
            }

            return establishment;
        }

        public void addEstablishment(int companyId, Establishment establishment)
        {
            _companies.FirstOrDefault(c => c.CompanyId == companyId)?.Establishments.Add(establishment);
            SaveChanges();
        }

        public void removeEstablishment(int establishmentId)
        {
            Establishment establishmentToDelete = _establishments.FirstOrDefault(c => c.EstablishmentId == establishmentId);
            if (establishmentToDelete != null)
            {
                establishmentToDelete.isDeleted = true;
                establishmentToDelete.Promotions.ForEach(p => p.isDeleted = true);
                establishmentToDelete.Events.ForEach(e => e.isDeleted = true);
                SaveChanges();
            }
        }

        public bool isOwnerOfEstablishment(int userId, int establishmentId)
        {
            return _establishments.Any(e => e.Company.MerchantId == userId && e.EstablishmentId == establishmentId && !e.isDeleted);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
