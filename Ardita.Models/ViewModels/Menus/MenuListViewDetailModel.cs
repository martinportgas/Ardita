using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Menus
{
    public class MenuListViewDetailModel
    {
        public Guid MenuId { get; set; }
        public Guid SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuPath { get; set; }
        public int SubMenuSort { get; set; }

    }
}
