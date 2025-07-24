using ClientDirectory.Domain.Enums;

namespace Application.Dtos.Client;

public class ClientDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public Genders? Gender { get; set; }
    public string? Identification { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool Status { get; set; }
}