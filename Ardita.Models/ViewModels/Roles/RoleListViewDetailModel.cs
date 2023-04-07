using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Roles
{
    public class RoleListViewDetailModel
    {
        public Guid RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

    }
}
