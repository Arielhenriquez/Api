using System.ComponentModel;

namespace Api.Domain.Enums;

public enum UserRoles
{
    [Description("Solicitante")]
    Applicant,

    [Description("Supervisor")]
    Supervisor,

    [Description("Administrativo")]
    Administrative,

    [Description("Administrador de área")]
    AreaAdministrator,

    [Description("Sudo")]
    Sudo,

    [Description("Chofer")]
    Driver
}