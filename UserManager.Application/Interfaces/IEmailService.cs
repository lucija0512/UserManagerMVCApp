using UserManager.Domain.Entities;

namespace UserManager.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(UserRecord user);
    }
}
