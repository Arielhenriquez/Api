namespace Api.Domain.Constants;

public static class EmailConstants
{
    public const string WelcomeUserTemplate = "WelcomeUser.html";
    public const string CreateDriverTemplate = "CreatedDriversEmailTemplate.html";
    public const string InventoryRequestTemplate = "InventoryRequestTemplate.html";
    public const string TransportRequestTemplate = "TransportRequestTemplate.html";
    public const string AssignedTransportRequestTemplate = "AssignedTransportRequestTemplate.html";
    public const string RejectedRequestTemplate = "RejectedRequestTemplate.html";
    public const string TemplateEmailRoute = "wwwroot/EmailTemplates";
}
