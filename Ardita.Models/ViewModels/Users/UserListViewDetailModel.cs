using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Users
{
    public class UserListViewDetailModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; }
        public bool IsActive { get; set; }
    }
}
