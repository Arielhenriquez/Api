using System.ComponentModel;

namespace Api.Domain.Enums;

public enum LocationType
{
    [Description("Ciudad")]
    City = 0,
    [Description("Interior")]
    Countryside
}
