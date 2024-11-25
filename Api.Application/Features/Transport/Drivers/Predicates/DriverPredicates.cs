﻿using Api.Application.Common.Extensions;
using Api.Domain.Entities.TransportEntities;
using Api.Domain.Enums;
using System.Linq.Expressions;

namespace Api.Application.Features.Transport.Drivers.Predicates;
//Todo: Fix enum filter.. if enum is 0 1 or null filter everything anyway
public static class DriverPredicates
{
    public static Expression<Func<Driver, bool>> Search(string criteria)
    {
        var matchingEnum = Enum.GetValues(typeof(DriverStatus))
           .Cast<Enum>()
           .FirstOrDefault(e => e.DisplayName().Contains(criteria, StringComparison.OrdinalIgnoreCase));

        if (matchingEnum != null)
        {
            var parsedStatus = (DriverStatus)matchingEnum;

            return transportEntity =>
                transportEntity.Status == parsedStatus;
        }
        else
        {
            return (Driver driver) =>
            driver.Name.Contains(criteria) ||
            driver.PhoneNumber.Contains(criteria) ||
            string.IsNullOrWhiteSpace(criteria);
        }
    }
}
