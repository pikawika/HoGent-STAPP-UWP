using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stapp.Data
{
    public class DummyDataInit
    {
        private readonly ApplicationDbContext _dbContext;

        public DummyDataInit(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                
                // SAVE CHANGES
                _dbContext.SaveChanges();
            }
        }
    }
}
