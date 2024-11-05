using System.ComponentModel;

namespace Api.Domain.Enums;

public enum DriverStatus
{
    [Description("Disponible")]
    Available,
    [Description("No Disponible")]
    Unavailable,
}
