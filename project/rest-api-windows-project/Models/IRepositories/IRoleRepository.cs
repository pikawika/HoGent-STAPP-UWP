using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stappBackend.Models.Domain;

namespace stappBackend.Models.IRepositories
{
    public interface IRoleRepository
    {
        Role GetByName(string name);
    }
}
