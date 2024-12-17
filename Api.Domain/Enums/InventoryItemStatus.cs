using System.ComponentModel;

namespace Api.Domain.Enums;

public enum InventoryItemStatus
{
    [Description("No Disponible")]
    StandBy = 0,
    [Description("Listo Para Despachar")]
    ReadyToDispatch = 1,
}
