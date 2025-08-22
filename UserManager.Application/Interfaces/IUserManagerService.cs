using UserManager.Application.DTOs;

namespace UserManager.Application.Interfaces
{
    public interface IUserManagerService
    {
        Task<ResultDTO> SaveUserFormAsync(string firstName, string lastName, string email);
    }
}
