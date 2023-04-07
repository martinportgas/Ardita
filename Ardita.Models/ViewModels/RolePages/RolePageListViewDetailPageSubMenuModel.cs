using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.RolePages
{
    public class RolePageListViewDetailPageSubMenuModel
    {
        public Guid PageId { get; set; }
        public string PageName { get; set; }
        public string PagePath { get; set; }
        public Guid SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuPath { get; set; }
    }
}
