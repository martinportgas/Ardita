using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.UserRoles
{
    public class UserRoleListViewDetailModel
    {
        public Guid UserRoleId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
}
