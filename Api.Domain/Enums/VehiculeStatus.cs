using System.ComponentModel;

namespace Api.Domain.Enums;

public enum VehiculeStatus
{
    [Description("Disponible")]
    Available,
    [Description("En taller")]
    InRepair,
    [Description("Fuera de servicio")]
    OutOfService,
}
