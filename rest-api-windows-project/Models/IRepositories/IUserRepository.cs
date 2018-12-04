using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface IUserRepository
    {
        User Login(string username, string hash);

        Boolean EmailExists(string email);

        Boolean UsernameExists(string username);

        User getById(int id);


        void Register(User user);

        void ChangePassword(int userId, byte[] newSalt, string newHash);

        byte[] GetSalt(string username);
    }
}
