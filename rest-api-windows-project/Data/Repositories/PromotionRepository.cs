using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly DbSet<Promotion> _promotions;
        private readonly DbSet<Establishment> _establishments;
        private readonly ApplicationDbContext _context;

        public PromotionRepository(ApplicationDbContext context)
        {
            _context = context;
            _promotions = context.Promotions;
            _establishments = context.Establishments;
        }

        public IEnumerable<Promotion> GetAll()
        {
            return _promotions
                .Where(p => !p.isDeleted)
                .Include(p => p.Images)
                .Include(p => p.Attachments)
                .Include(p => p.Establishment).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(p => p.Establishment).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(p => p.Establishment).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(p => p.Establishment).ThenInclude(e => e.ExceptionalDays)
                .ToList();
        }

        public Promotion getById(int id)
        {
            return _promotions.Where(p => p.PromotionId == id && !p.isDeleted)
                .Include(p => p.Images)
                .Include(p => p.Attachments)
                .Include(p => p.Establishment).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(p => p.Establishment).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(p => p.Establishment).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(p => p.Establishment).ThenInclude(e => e.ExceptionalDays)
                .FirstOrDefault();
        }

        public void addPromotion(int establishmentId, Promotion newPromotion)
        {
            _establishments.FirstOrDefault(e => e.EstablishmentId == establishmentId)?.Promotions.Add(newPromotion);
            SaveChanges();
        }

        public void removePromotion(int promotionId)
        {
            Promotion promotionToDelete = _promotions.FirstOrDefault(c => c.PromotionId == promotionId);
            if (promotionToDelete != null)
            {
                promotionToDelete.isDeleted = true;
                SaveChanges();
            }
        }

        public bool isOwnerOfPromotion(int userId, int promotionId)
        {
            return _promotions.Any(p => !p.isDeleted && p.PromotionId == promotionId && p.Establishment.Company.Merchant.UserId == userId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
