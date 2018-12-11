using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface IEstablishmentRepository
    {
        IEnumerable<Establishment> GetAll();

        Establishment getById(int id);

        void addEstablishment(int companyId, Establishment establishment);

        void removeEstablishment(int establishmentId);

        void SaveChanges();
    }
}
