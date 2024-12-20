﻿using Api.Application.Features.Transport.TransportRequest.Dtos;
using FluentValidation;

namespace Api.Application.Features.Transport.TransportRequest.Validators;

public class TransportRequestValidators : AbstractValidator<TransportRequestDto>
{
    public TransportRequestValidators()
    {
        RuleFor(x => x.DeparturePoint).NotNull().NotEmpty();
        RuleFor(x => x.Destination).NotNull().NotEmpty();
        RuleFor(x => x.NumberOfPeople).NotNull().NotEmpty();
        RuleFor(x => x.DepartureDateTime)
                   .NotNull()
                   .NotEmpty()
                   .GreaterThanOrEqualTo(DateTime.Now.Date)
                   .WithMessage("La fecha de salida debe ser hoy o una fecha futura.");

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .Length(12, 22)
            .Matches(@"^(809|829|849)-\d{3}-\d{4}( Ext:\d{4})?$")
            .WithMessage("El número de teléfono debe iniciar con 809, 829 o 849, tener el formato 809-123-4567, y opcionalmente una extensión en el formato 'Ext: 7000'.");
    }
}


public class AssignTransportValidators : AbstractValidator<AssignDriverVehicleDto>
{
    public AssignTransportValidators()
    {
        RuleFor(x => x.DriverId).NotNull().NotEmpty();
        RuleFor(x => x.VehicleId).NotNull().NotEmpty();
    }
}
