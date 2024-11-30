namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;

public class LoggedUser
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public ICollection<string>? Roles { get; set; }
    public string? Oid { get; set; }
}
