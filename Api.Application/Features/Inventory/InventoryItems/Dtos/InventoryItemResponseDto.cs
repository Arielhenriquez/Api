using Api.Domain.Entities.InventoryEntities;

namespace Api.Application.Features.Inventory.InventoryItems.Dtos;

public class InventoryItemResponseDto
{
    public Guid Id { get; set; }
    public int InstitutionalCode { get; set; }
    public required string Category { get; set; }
    public required string WarehouseObjectAccount { get; set; }
    public required string AcquisitionObjectAccount { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public required int RequestedQuantity { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal? Value { get; set; }
    public required string Section { get; set; }
    public string? DeleteComment { get; set; }

    public static implicit operator InventoryItemResponseDto(InventoryItem inventoryItem)
    {
        return inventoryItem is null
        ? null
        : new InventoryItemResponseDto
        {
            Id = inventoryItem.Id,
            InstitutionalCode = inventoryItem.InstitutionalCode,
            Category = inventoryItem.Category,
            WarehouseObjectAccount = inventoryItem.WarehouseObjectAccount,
            AcquisitionObjectAccount = inventoryItem.AcquisitionObjectAccount,
            Name = inventoryItem.Name,
            Quantity = inventoryItem.Quantity,
            RequestedQuantity = inventoryItem.RequestedQuantity,
            UnitOfMeasure = inventoryItem.UnitOfMeasure,
            Value = inventoryItem.Value,
            Section = inventoryItem.Section,
            DeleteComment = inventoryItem.DeleteComment,
        };
    }
}
