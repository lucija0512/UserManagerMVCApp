using UserManager.Domain.Entities;

namespace UserManager.Application.DTOs
{
    public record UserDetailsDTO(string? Username, string? Phone, string? Website, Address? Address, Company? Company);
}
