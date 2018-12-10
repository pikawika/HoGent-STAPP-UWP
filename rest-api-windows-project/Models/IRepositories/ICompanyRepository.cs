using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface ICompanyRepository
    {
        void addCompany(int userId, Company company);

        Company getById(int companyId);

        void removeCompany(int companyId);

        void SaveChanges();
    }
}
