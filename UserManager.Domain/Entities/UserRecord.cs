namespace UserManager.Domain.Entities
{
    public class UserRecord
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }        
        public string? Website { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Address? Address { get; set; }
        public Company? Company { get; set; }
    }
}
