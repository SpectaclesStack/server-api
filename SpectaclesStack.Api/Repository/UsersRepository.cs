
using spectaclesStackServer.Data;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Model;
using spectaclesStackServer.Dto;
using Microsoft.AspNetCore.Mvc;

namespace spectaclesStackServer.Repository
{
    public class UsersRepository : IUsersRepository

    {
        private readonly DataContext context;

        public UsersRepository(DataContext context)
        {
            this.context = context;
        }
        public bool UserExists(int userId)
        {
            return context.Users.Any(u => u.UserId == userId);
        }

        public bool CreateUser(Users user)
        {
            context.Add(user);
            return Save();
        }

        public bool DeleteUser(Users user)
        {

            context.Remove(user);
            return Save();
        }

        public ICollection<Users> GetUsers()
        {
            return context.Users.ToList();
        }

        public Users GetUser(int userId)
        {
            
            return context.Users.Where(u => u.UserId == userId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateUser(Users users)
        {
            context.Update(users);
            return Save();
        }
    }
}