namespace UserManager.Infrastructure.ExternalServices.Api.Models
{
    internal class CompanyApiResponse
    {
        public required string Name { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }
    }
}
