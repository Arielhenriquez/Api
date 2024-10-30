using System.ComponentModel;

namespace Api.Domain.Enums;

public enum RequestStatus
{
    [Description("Pendiente")]
    Pending = 0,
    [Description("Aprobado")]
    Approved,
    [Description("Rechazado")]
    Rejected,
    [Description("Despachado")]
    Dispatched

}
