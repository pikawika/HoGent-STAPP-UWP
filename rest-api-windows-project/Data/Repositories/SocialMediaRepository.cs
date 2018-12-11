using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class SocialMediaRepository : ISocialMediaRepository
    {
        private readonly DbSet<SocialMedia> _socialMedias;

        public SocialMediaRepository(ApplicationDbContext context)
        {
            _socialMedias = context.SocialMedias;
        }

        public SocialMedia getByName(string Name)
        {
            return _socialMedias.FirstOrDefault(sm => sm.Name == Name);
        }
    }
}
