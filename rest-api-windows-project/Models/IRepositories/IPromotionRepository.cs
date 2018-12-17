using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface IPromotionRepository
    {
        IEnumerable<Promotion> GetAll();
        Promotion getById(int id);

        void addPromotion(int establishmentId, Promotion newPromotion);

        void removePromotion(int promotionId);

        Boolean isOwnerOfPromotion(int userId, int promotionId);

        void SaveChanges();
    }
}
