using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels
{
    public class DataTableModel
    {
        public string? draw { get; set; }
        public string? start { get; set; }
        public string? length { get; set; }
        public string? sortColumn { get; set; }
        public string? sortColumnDirection { get; set; }
        public string? searchValue { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public int recordsTotal { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? SubMenuId { get; set; }
        public Guid? EmployeeId { get; set; }

    }
}
