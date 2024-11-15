using Api.Application.Features.Inventory.InventoryRequest.Dtos;
using FluentValidation;

namespace Api.Application.Features.Inventory.InventoryRequest.Validators;

public class InventoryRequestValidators : AbstractValidator<InventoryRequestDto>
{
    public InventoryRequestValidators()
    {
        RuleFor(x => x.CollaboratorId).NotNull().NotEmpty();
        RuleFor(x => x.ArticlesIds).NotNull().NotEmpty();
    }
}
