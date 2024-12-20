using System.ComponentModel;

namespace Api.Domain.Enums;

public enum TransportRequestStatus
{
    [Description("Pendiente")]
    Pending = 0,
    [Description("En Proceso")]
    InProcess,
    [Description("Rechazado/Cancelado")]
    Rejected,
    [Description("Completado")]
    Completed
}
