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
        private readonly ApplicationDbContext _context;

        public PromotionRepository(ApplicationDbContext context)
        {
            _context = context;
            _promotions = context.Promotions;
        }

        public IEnumerable<Promotion> GetAll()
        {
            return _promotions
                .Include(p => p.Images)
                .ToList();
        }

        public Promotion getById(int id)
        {
            return _promotions.Where(p => p.PromotionId == id)
                .Include(p => p.Images)
                .FirstOrDefault();
        }
    }
}
