using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User GetById(int id);
        void Update(User user);
    }
}
