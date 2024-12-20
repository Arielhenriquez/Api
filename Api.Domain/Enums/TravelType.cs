using System.ComponentModel;

namespace Api.Domain.Enums;

public enum TravelType
{
    [Description("Traslado de personas")]
    PassengerTransport = 0,
    [Description("Carga")]
    CargoTransport
}
