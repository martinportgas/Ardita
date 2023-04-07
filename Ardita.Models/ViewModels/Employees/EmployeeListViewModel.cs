using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Employees
{
    public class EmployeeListViewModel
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<EmployeeListViewDetailModel> data { get; set; }
    }
}
