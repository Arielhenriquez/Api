namespace Api.Domain.Settings;

public class GraphSettings
{
    public string? ServicePrincipalId { get; set; }
    public required string ClientId { get; set; }
    public required string TenantId { get; set; }
    public required string ClientSecret { get; set; }
}
