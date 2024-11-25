namespace Api.Application.Features.Collaborators.Dtos;

public class GraphUserDto
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }
    public string Mail { get; set; }
    public string GivenName { get; set; }
    public string SurName { get; set; }
    public string Department { get; set; }
    public ManagerDto? ManagerDto { get; set; } // Corregido
}

public class ManagerDto
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }
    public string Mail { get; set; }
}
//Todo: change name to app role response dto.. move to another folder jj
public class AppRoleDto
{
    public Guid? Id { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
}
