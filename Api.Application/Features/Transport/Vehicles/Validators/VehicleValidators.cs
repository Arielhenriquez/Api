using Api.Application.Features.Transport.Vehicles.Dtos;
using FluentValidation;

namespace Api.Application.Features.Transport.Vehicles.Validators;

public class VehicleValidators : AbstractValidator<VehicleRequestDto>
{
    public VehicleValidators()
    {
        RuleFor(x => x.Brand).NotNull().NotEmpty();
        RuleFor(x => x.Model).NotNull().NotEmpty();
        RuleFor(x => x.LicensePlate).NotNull().NotEmpty();
        RuleFor(x => x.InsuranceValidity).NotNull().NotEmpty();
        RuleFor(x => x.InsuranceType).NotNull().NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}
