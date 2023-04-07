using Ardita.Models.ViewModels.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Roles
{
    public class RoleListViewModel
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<RoleListViewDetailModel> data { get; set; }
    }
}
