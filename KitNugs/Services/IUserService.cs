using KitNugs.Controllers;
using KitNugs.Services.Model;

namespace KitNugs.Services
{
    public interface IUserService
    {
        Task CreateUser(string tenantId, UserResponse userToCreate);
        Task<IList<UserResponse>> GetUsersForTenant(string tenantId);
    }
}
