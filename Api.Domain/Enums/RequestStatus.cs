using System.ComponentModel;

namespace Api.Domain.Enums;

public enum RequestStatus
{
    [Description("Pendiente")]
    Pending = 0,
    [Description("Aprobado por Supervisor")]
    Approved,
    [Description("Aprobado por Administrador")]
    ApprovedByAdmin,
    [Description("Rechazado")]
    Rejected,
    [Description("Despachado")]
    Dispatched
}
