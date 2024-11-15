using Api.Application.Features.Inventory.InventoryItems.Dtos;
using FluentValidation;

namespace Api.Application.Features.Inventory.InventoryItems.Validators;

public class InventoryItemValidators : AbstractValidator<InventoryItemRequestDto>
{
    public InventoryItemValidators()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Quantity).NotNull().NotEmpty();
    }
}
