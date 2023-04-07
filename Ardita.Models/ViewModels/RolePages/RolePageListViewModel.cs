using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.RolePages
{
    public class RolePageListViewModel
    {
        public MstRolePage rolePage { get; set; }
        public MstRole role { get; set; }
        public MstPage page { get; set; }
        public IEnumerable<RolePageListViewDetailPageSubMenuModel> rolePageSubMenu { get; set; }
        public IEnumerable<RolePageListViewDetailModel> rolePages { get; set; }
    }
}
