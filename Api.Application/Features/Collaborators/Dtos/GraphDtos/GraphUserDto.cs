namespace Api.Application.Features.Collaborators.Dtos.GraphDtos;
public class GraphUserDto
{
    public required string Id { get; set; }
    public  required string Name { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Department { get; set; }
    public ManagerDto? Manager { get; set; }
}

public class ManagerDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
