using KitNugs.Controllers;
using KitNugs.Services.Model;

namespace KitNugs.Services
{
    public interface IUserService
    {
        Task CreateUser(UserResponse userToCreate);
        Task<IList<UserResponse>> GetUsers();
    }
}
