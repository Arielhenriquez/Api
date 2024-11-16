using Api.Application.Features.Transport.Drivers.Dtos;
using FluentValidation;

namespace Api.Application.Features.Transport.Drivers.Validators;

public class DriverValidators : AbstractValidator<DriverRequestDto>
{
    public DriverValidators()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Status).NotNull().NotEmpty().IsInEnum();
        RuleFor(x => x.LicenseExpiration)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.Date)
            .WithMessage("La fecha de vencimiento debe ser hoy o una fecha futura.");

        RuleFor(x => x.PhoneNumber)
        .NotNull()
        .NotEmpty()
        .Length(12, 22)
        .Matches(@"^(809|829|849)-\d{3}-\d{4}( Ext:\s?\d{4})?$")
        .WithMessage("El número de teléfono debe iniciar con 809, 829 o 849, tener el formato 809-123-4567, y opcionalmente una extensión en el formato 'Ext: 7000'.");

    }
}
