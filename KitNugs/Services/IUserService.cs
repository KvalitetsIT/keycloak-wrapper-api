using KitNugs.Controllers;
using KitNugs.Services.Model;

namespace KitNugs.Services
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(UserResponse userToCreate);
        Task<IList<UserResponse>> GetUsers();
    }
}
