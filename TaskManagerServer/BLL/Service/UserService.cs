using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DLL.Repositories;
using Domain.Models;

namespace BLL.Service
{
    public class UserService : IService<User>
    {
        public IRepository<User> userRepository { get; set; }

        public UserService(IRepository<User> repository) 
        {
            userRepository = repository;
        }
        public IQueryable<User> GetAll() => userRepository.GetAll();
        public async Task AddAsync(User user) => await userRepository.AddAsync(user);
        public async Task DeleteAsync(User user) => await userRepository.DeleteAsync(user.Id);
        public IQueryable<User> GetByCondition(Expression<Func<User, bool>> predicate) => userRepository.GetByCondition(predicate);
        public async Task UpdateAsync(User newEntity, int oldId) => await userRepository.UpdateAsync(newEntity, oldId);

        public User FindUserByLoginAndPassword(User user)
        {
            var users = GetByCondition(u => u.Login == user.Login && u.PasswordHash == user.PasswordHash);
            if (users.Any())
                return users.First();
            else 
                return null;
        }

        public List<User> FindByMatchingLogin(string login)
        {
            return GetByCondition(u => Regex.IsMatch(u.Login, login)).ToList();
        }

    }
}
