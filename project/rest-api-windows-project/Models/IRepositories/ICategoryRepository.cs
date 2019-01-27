using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface ICategoryRepository
    {
        Category GetByName(string name);

        void Add(Category category);
    }
}
