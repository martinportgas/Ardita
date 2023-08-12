using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.LogModels
{
    public class LogRequestModel
    {
      public string TableName { get; set; } 
      public LogRequestDetailModel data { get; set; }
    }

    public class LogRequestDetailModel
    {
        public object Header { get; set; }
        public object Child { get; set; }
    }
}
