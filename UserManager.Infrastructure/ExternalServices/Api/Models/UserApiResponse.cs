namespace UserManager.Infrastructure.ExternalServices.Api.Models
{
    internal class UserApiResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public  required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public AddressApiResponse? Address { get; set; }
        public CompanyApiResponse? Company { get; set; }
    }
}
