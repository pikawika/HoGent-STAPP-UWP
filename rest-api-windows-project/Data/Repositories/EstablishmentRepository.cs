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
            return _establishments.OrderBy(e => (e.Promotions.Count() + e.Events.Count()))
                .Include(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Images)
                .Include(e => e.ExceptionalDays)
                .Include(e => e.Promotions).ThenInclude(p => p.Images)
                .Include(e => e.Events).ThenInclude(e => e.Images)
                .ToList();
        }

        public Establishment getById(int id)
        {
            return _establishments.Where(e => e.EstablishmentId == id)
                .Include(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Images)
                .Include(e => e.ExceptionalDays)
                .Include(e => e.Promotions).ThenInclude(p => p.Images)
                .Include(e => e.Events).ThenInclude(e => e.Images)
                .FirstOrDefault();
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
                establishmentToDelete.isDeleted = true;
        }

        public bool isOwnerOfEstablishment(int userId, int establishmentId)
        {
            return _establishments.Any(c => c.Company.MerchantId == userId && c.EstablishmentId == establishmentId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
