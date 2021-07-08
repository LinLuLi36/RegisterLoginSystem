using RegisterLoginSystem.Models;
using System.Collections.Generic;

namespace RegisterLoginSystem.Repository
{
    public interface IUserRepository
    {
        public void AddUser(User user);
        public void UpdateUser(User user);
        public IEnumerable<User> GetUsers();

    }
}
