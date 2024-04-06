using SpectablesStack.Api.Interface;
using AutoMapper;
using stackup_vsc_setup.Model;
using stackup_vsc_setup.Data;

namespace SpectablesStack.Api.Repository
{
    public class UsersRepository : IUsersRepository

    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UsersRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public bool UserExists(int userId)
        {
            return context.Users.Any(u => u.UserId == userId);
        }

        public bool CreateUser(Users users)
        {
            context.Add(users);
            return Save();
        }

        public bool DeleteUser(Users users)
        {
            context.Remove(users);
            return Save();
        }

        public ICollection<Users> GetUsers()
        {
            return context.Users.OrderBy(u => u.UserId ).ToList();
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