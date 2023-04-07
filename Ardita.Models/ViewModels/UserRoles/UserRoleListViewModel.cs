using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.UserRoles
{
    public class UserRoleListViewModel
    {
        public MstUserRole  UserRole { get; set; }
        public IEnumerable<UserRoleListViewDetailModel> UserRoles { get; set; }
        public IEnumerable<MstRole> Roles { get; set; }
        public MstUser Users { get; set; }
    }
}
