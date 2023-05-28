using Ardita.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ardita.Extensions
{
    public static class AppUsers
    {
        public static SessionModel CurrentUser(this ClaimsPrincipal claims)
        {
            Guid userId = Guid.Empty;
            Guid roleId = Guid.Empty;
            Guid positionId = Guid.Empty;
            Guid companyId = Guid.Empty;
            Guid employeeId = Guid.Empty;

            Guid.TryParse(claims.FindFirst(GlobalConst.UserId)!.Value, out userId);
            Guid.TryParse(claims.FindFirst(GlobalConst.RoleId)!.Value, out roleId);
            Guid.TryParse(claims.FindFirst(GlobalConst.PositionId)!.Value, out positionId);
            Guid.TryParse(claims.FindFirst(GlobalConst.CompanyId)!.Value, out companyId);
            Guid.TryParse(claims.FindFirst(GlobalConst.EmployeeId)!.Value, out employeeId);

            SessionModel session = new SessionModel();
            session.UserId = userId;
            session.Username = claims.FindFirst(GlobalConst.Username)!.Value;
            session.RoleId = roleId;
            session.RoleCode = claims.FindFirst(GlobalConst.RoleCode)!.Value;
            session.RoleName = claims.FindFirst(GlobalConst.RoleName)!.Value;
            session.EmployeeNIK = claims.FindFirst(GlobalConst.EmployeeNIK)!.Value;
            session.EmployeeName = claims.FindFirst(GlobalConst.EmployeeName)!.Value;
            session.PositionId = positionId;
            session.CompanyId = companyId;
            session.CompanyName = claims.FindFirst(GlobalConst.CompanyName)!.Value;
            session.EmployeeId = employeeId;
            session.ListArchiveUnitCode = new List<string>();

            if (claims.FindFirst(GlobalConst.ArchiveUnitCode) != null)
            {
                string strArchiveUnitCode = claims.FindFirst(GlobalConst.ArchiveUnitCode)!.Value;
                session.ListArchiveUnitCode = string.IsNullOrEmpty(strArchiveUnitCode) ? new List<string>() : strArchiveUnitCode.Split(",").ToList();
            } 

            return session;
        }
    }
}
