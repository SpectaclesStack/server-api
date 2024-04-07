using spectaclesStackServer.Model;
using spectaclesStackServer.Dto;
namespace spectaclesStackServer.Interface
{
    public interface IUsersRepository
    {
        ICollection<Users> GetUsers();
        Users GetUser(int userId);
        bool UserExists(int userId);
        bool CreateUser(Users users); // wrap responses around service response record class
        bool UpdateUser(Users users);
        bool DeleteUser(Users users);
        bool Save();
    }
}
        
    