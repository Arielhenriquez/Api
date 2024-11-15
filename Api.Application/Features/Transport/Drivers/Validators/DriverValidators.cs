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
            .Matches(@"^(809|829|849)\d{7}(\d{4})?$")
            .WithMessage("El número de teléfono debe iniciar con 809, 829 o 849, tener 10 dígitos, y opcionalmente una extensión de 4 dígitos.");
    }
}
