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

            Guid.TryParse(claims.FindFirst("UserId").Value, out userId);
            Guid.TryParse(claims.FindFirst("RoleId").Value, out roleId);
            Guid.TryParse(claims.FindFirst("PositionId").Value, out positionId);

            SessionModel session = new SessionModel();
            session.UserId = userId;
            session.Username = claims.FindFirst("Username").Value;
            session.RoleId = roleId;
            session.RoleCode = claims.FindFirst("RoleCode").Value;
            session.RoleName = claims.FindFirst("RoleName").Value;
            session.EmployeeNIK = claims.FindFirst("EmployeeNIK").Value;
            session.EmployeeName = claims.FindFirst("EmployeeName").Value;
            session.PositionId = positionId;

            return session;
        }
    }
}
