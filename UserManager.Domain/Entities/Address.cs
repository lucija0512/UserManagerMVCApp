namespace UserManager.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public required string Street { get; set; }
        public required string Suite { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }
}
