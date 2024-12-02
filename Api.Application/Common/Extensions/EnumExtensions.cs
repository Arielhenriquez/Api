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
    public static UserRoles? MapDbRoleToEnum(string dbRole)
    {
        return dbRole switch
        {
            "Solicitante.ReadWrite" => UserRoles.Supervisor,
            "Supervisor.Approval" => UserRoles.Applicant,
            "Admin.Approval" => UserRoles.Administrative,
            "AdminDeArea.ReadWrite" => UserRoles.AreaAdministrator,
            "Sudo.All" => UserRoles.Sudo,
            "Chofer.Read" => UserRoles.Driver,
            _ => null
        };
    }
}
