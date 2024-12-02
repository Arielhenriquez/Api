using System.Data;
using Api.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
//Todo: probar roles y acota eto jj
namespace Api.Application.Common;

//[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
//public class RoleAuthorizeAttribute : AuthorizeAttribute
//{
//    public RoleAuthorizeAttribute(params UserRoles[] roles)
//    {
//        Roles = string.Join(",", roles.Select(r => r.ToString()));
//    }
//}
