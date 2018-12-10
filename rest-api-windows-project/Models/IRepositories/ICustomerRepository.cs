using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface ICustomerRepository
    {
        void addSubscription(int userId, EstablishmentSubscription establishmentSubscription);

        void removeSubscription(int userId, EstablishmentSubscription establishmentSubscription);

        Customer getById(int userId);

        List<Establishment> GetEstablishmentSubscriptions(int userId);

    }
}
