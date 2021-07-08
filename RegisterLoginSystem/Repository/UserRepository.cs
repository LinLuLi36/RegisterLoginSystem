using RegisterLoginSystem.Dal;
using RegisterLoginSystem.Models;
using System.Collections.Generic;

namespace RegisterLoginSystem.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly Entities _entities;

        public UserRepository(Entities entities)
        {
            _entities = entities;
        }

        public void AddUser(User user)
        {
            _entities.Users.Add(user);
            _entities.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _entities.Users.Update(user);
            _entities.SaveChanges();
        }

        public IEnumerable<User> GetUsers()
        {
            return _entities.Users;
        }
    }
}
