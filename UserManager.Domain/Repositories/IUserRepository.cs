using UserManager.Domain.Entities;

namespace UserManager.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CanInsertUserAsync(string email);
        Task<bool> InsertUserRecordAsync(UserRecord user);
    }
}
