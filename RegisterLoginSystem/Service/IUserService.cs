using RegisterLoginSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterLoginSystem.Service
{
    public interface IUserService
    {
        public User GetUserByEmail(string email);
        public void UpdateUser(User user);
        public void AddUser(User user);
        public List<User> GetUsersByEmailAndPassword(string email, string password);
        public List<User> GetAllUsers();

    }
}
