using UserManager.Application.DTOs;
using UserManager.Application.Interfaces;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJsonPlaceholderService _jsonPlaceholderService;
        private readonly IEmailService _emailService;

        public UserManagerService(
            IUserRepository userRepository, 
            IJsonPlaceholderService jsonPlaceholderService, 
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _jsonPlaceholderService = jsonPlaceholderService;
            _emailService = emailService;
        }

        public async Task<ResultDTO> SaveUserFormAsync(string firstName, string lastName, string email)
        {
            // Check if user was inserted in the last minute
            if (!await _userRepository.CanInsertUserAsync(email))
            {
                return ResultDTO.Failure(["Od posljednjeg unosa istog korisnika je prošlo manje od minute. Pokušajte kasnije."]);
            }
            // Get user details from external api
            var userDetails = await _jsonPlaceholderService.GetUserDetailsAsync(email);

            var user = new UserRecord { 
                FirstName = firstName, 
                LastName = lastName, 
                Email = email, 
                Phone = userDetails?.Phone, 
                Website = userDetails?.Website,
                Username = userDetails?.Username,
                Address = userDetails?.Address,
                Company = userDetails?.Company,
            };

            // Insert user into database
            if (!await _userRepository.InsertUserRecordAsync(user))
            {
                return ResultDTO.Failure(["Upis korisnika nije uspio."]);
            }
            // Send email to user
            if (!await _emailService.SendEmail(user))
            {
                return ResultDTO.Failure(["Korisnik je upisan, ali email nije uspješno poslan."]);
            }
            return ResultDTO.Success();
        }
    }
}
