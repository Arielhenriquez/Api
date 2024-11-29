namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;

public class GraphUserDto
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }
    public string Mail { get; set; }
    public string GivenName { get; set; }
    public string SurName { get; set; }
    public string Department { get; set; }
    public ManagerDto? ManagerDto { get; set; }
}

public class ManagerDto
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }
    public string Mail { get; set; }
}

public class LoggedUser
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public ICollection<string>? Roles { get; set; }
    public string? Oid { get; set; }
}
