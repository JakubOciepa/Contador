using Contador.Core.Models;

namespace Contador.Api.Services.Interfaces
{
    /// <summary>
    /// Users manager.
    /// </summary>
    public interface IUserService
    {
        User GetUserById(int id);
    }
}
