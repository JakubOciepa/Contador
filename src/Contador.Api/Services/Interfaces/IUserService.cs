using Contador.Core.Models;

namespace Contador.Api.Services
{
    /// <summary>
    /// Users manager.
    /// </summary>
    public interface IUserService
    {
        User GetUserById(int id);
    }
}
