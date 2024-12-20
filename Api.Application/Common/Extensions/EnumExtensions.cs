using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using Api.Domain.Enums;

namespace Api.Application.Common.Extensions;

public static class EnumExtensions
{
    private static readonly
       ConcurrentDictionary<string, string> DisplayNameCache = new();

    public static string DisplayName(this Enum value)
    {
        var key = $"{value.GetType().FullName}.{value}";

        var displayName = DisplayNameCache.GetOrAdd(key, x =>
        {
            var name = (DescriptionAttribute[])value
                .GetType()
                .GetTypeInfo()
                .GetField(value.ToString())!
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return name.Length > 0 ? name[0].Description : value.ToString();
        });

        return displayName;
    }

    public static Dictionary<string, InventoryRequestStatus> GetRequestStatusMap()
    {
        return Enum.GetValues(typeof(InventoryRequestStatus))
            .Cast<InventoryRequestStatus>()
            .ToDictionary(
                status => status.DisplayName(),
                status => status);
    }

    public static Dictionary<string, TransportRequestStatus> GetTransportRequestStatusMap()
    {
        return Enum.GetValues(typeof(TransportRequestStatus))
            .Cast<TransportRequestStatus>()
            .ToDictionary(
                status => status.DisplayName(),
                status => status);
    }

    public static UserRoles? MapDbRoleToEnum(string dbRole)
    {
        return dbRole switch
        {
            "Solicitante.ReadWrite" => UserRoles.Applicant,
            "Supervisor.Approval" => UserRoles.Supervisor,
            "Admin.Approval" => UserRoles.Administrative,
            "AdminDeArea.ReadWrite" => UserRoles.AreaAdministrator,
            "AdminDeAreaTrans.ReadWrite" => UserRoles.AreaAdministratorTrans,
            "Sudo.All" => UserRoles.Sudo,
            "Chofer.Read" => UserRoles.Driver,
            _ => null
        };
    }
    public static string MapEnumToDbRole(UserRoles role)
    {
        return role switch
        {
            UserRoles.Applicant => "Solicitante.ReadWrite",
            UserRoles.Supervisor => "Supervisor.Approval",
            UserRoles.Administrative => "Admin.Approval",
            UserRoles.AreaAdministrator => "AdminDeArea.ReadWrite",
            UserRoles.AreaAdministratorTrans => "AdminDeAreaTrans.ReadWrite",
            UserRoles.Sudo => "Sudo.All",
            UserRoles.Driver => "Chofer.Read",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}
