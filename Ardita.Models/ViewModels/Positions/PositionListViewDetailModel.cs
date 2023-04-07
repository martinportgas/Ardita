using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Positions
{
    public class PositionListViewDetailModel
    {
        public Guid PositionId { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public bool IsActive { get; set; }

    }
}
