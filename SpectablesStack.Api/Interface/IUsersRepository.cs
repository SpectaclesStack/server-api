using stackup_vsc_setup.Model;

namespace SpectablesStack.Api.Interface
{
    public interface IUsersRepository
    {
        ICollection<Users> GetUsers();
        Users GetUser(int userId);
        bool UserExists(int userId);
        bool CreateUser(Users users);
        bool UpdateUser(Users users);
        bool DeleteUser(Users users);
        bool Save();
    }
}
        
    