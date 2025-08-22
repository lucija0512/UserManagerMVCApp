namespace UserManager.Infrastructure.ExternalServices.Api.Models
{
    internal class AddressApiResponse
    {
        public required string Street { get; set; }
        public required string Suite { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public GeoApiResponse? Geo { get; set; }
    }
}
