using System.ComponentModel;

namespace Api.Domain.Enums;

public enum PendingApprovalBy
{
    [Description("")]
    None = 0,
    [Description("Supervisor")]
    Supervisor,

    [Description("Administrativo")]
    Administrative,

    [Description("Administrador de área")]
    AreaAdministrator,

    [Description("Sudo")]
    Sudo,
}
