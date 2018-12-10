using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _users;
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _users = context.Users;
        }

        public User Login(string username, string hash)
        {
            return _users.Where(u => u.Login.Username == username && u.Login.Hash == hash)
                .Include(u => u.Login).ThenInclude(l => l.Role)
                .FirstOrDefault();
        }

        public User getById(int id)
        {
            return _users.Where(u => u.UserId == id)
                .Include(u => u.Login)
                .FirstOrDefault();
        }

        public bool EmailExists(string email)
        {
            return _users.Any(g => g.Email == email);
        }

        public bool UsernameExists(string username)
        {
            return _users.Any(g => g.Login.Username == username);
        }

        public void Register(User user)
        {
            _users.Add(user);
            SaveChanges();
        }

        public void ChangePassword(int userId, byte[] newSalt, string newHash)
        {
            User user = _users.Where(u => u.UserId == userId).Include(u => u.Login).FirstOrDefault();

            if (user != null)
            {
                user.Login.Salt = newSalt;
                user.Login.Hash = newHash;
                SaveChanges();
            }
        }

        public void ChangeUsername(int userId, string newUsername)
        {
            User user = _users.Where(u => u.UserId == userId).Include(u => u.Login).FirstOrDefault();

            if (user != null && !UsernameExists(newUsername))
            {
                user.Login.Username = newUsername;
                SaveChanges();
            }
        }

        public byte[] GetSalt(string username)
        {
            return _users.Where(u => u.Login.Username == username).Include(u => u.Login).FirstOrDefault()?.Login.Salt;
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
