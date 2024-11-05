using Api.Application.Interfaces.Inventory;

namespace Api.Application.Features.Inventory.InventoryRequest.Services;

public class InventoryRequestService : IInventoryRequestService
{
    /*Todo: 1- Recibir colaborator Id y buscarlo para ver si existe.. si no existe crealo(por ahora)
    2- Status pendiente y fecha date time.now
    3- crear inventory request
    4- Recibir array de articulos Id y el Id de la solicitud creada ( validar que articulo exista por el id)
    5- Crear inventory request item     
    6- Enviar correo para aprobar soliciud
    7- Endpoint para que un Supervisor pueda aprobar o rechazar solicitud  */
    
    public async Task AddInventoryRequest()
    {
        throw new NotImplementedException();
    }
}
