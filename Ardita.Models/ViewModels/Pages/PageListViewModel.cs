using Ardita.Models.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Pages
{
    public class PageListViewModel
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<PageListViewDetailModel> data { get; set; }
    }
}
