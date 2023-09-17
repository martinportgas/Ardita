using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models
{
    public class GlobalSearchModel
    {
        public int? StatusId { get; set; }
        public bool? IsArchiveActive { get; set; }
        public Guid? ArchiveUnitId { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
