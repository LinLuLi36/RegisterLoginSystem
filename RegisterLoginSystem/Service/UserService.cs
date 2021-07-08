using RegisterLoginSystem.Models;
using RegisterLoginSystem.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RegisterLoginSystem.Service
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserByEmail (string email)
        {
            return _userRepository.GetUsers().FirstOrDefault(s => s.Email == email);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }
        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public List<User> GetUsersByEmailAndPassword (string email, string password)
        {
            return _userRepository.GetUsers().Where(s => s.Email.Equals(email) && s.Password.Equals(password)).ToList();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetUsers().ToList();
        }
    }
}
