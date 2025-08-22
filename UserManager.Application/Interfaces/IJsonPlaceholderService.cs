using UserManager.Application.DTOs;

namespace UserManager.Application.Interfaces
{
    public interface IJsonPlaceholderService
    {
        Task<UserDetailsDTO?> GetUserDetailsAsync(string email);
    }
}
