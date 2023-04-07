using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Users
{
    public class UserListViewModel
    {
      public string draw { get; set; }
      public int recordsFiltered { get; set; }
      public int recordsTotal { get; set; }
      public List<UserListViewDetailModel> data { get; set; }
    }
}
