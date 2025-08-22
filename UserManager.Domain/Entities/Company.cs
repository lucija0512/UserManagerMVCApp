namespace UserManager.Domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }
    }
}
