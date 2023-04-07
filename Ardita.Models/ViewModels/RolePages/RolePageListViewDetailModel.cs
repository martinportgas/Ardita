using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.RolePages
{
    public class RolePageListViewDetailModel
    {
        public Guid RolePageId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public Guid PageId { get; set; }
        public string PageName { get; set; }
        public string pagePath { get; set; }
        public Guid SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuPath { get; set; }

    }
}
