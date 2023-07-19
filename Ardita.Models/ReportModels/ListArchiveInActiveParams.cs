using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ReportModels
{
    public class ListArchiveInActiveParams
    {
        public Guid? companyId { get; set; }
        public Guid? archiveUnit { get; set; }
        public Guid? roomId { get; set; }
        public Guid? rackId { get; set; }
        public Guid? levelId { get; set; }
        public Guid? rowId { get; set; }
        public Guid? statusId { get; set; }
        public Guid? gmdId { get; set; }
        public Guid? creatorId { get; set; }
        public Guid? ownerId { get; set; }
        public Guid? clasificationId { get; set; }
        public Guid? subjectClasification { get; set; }
        public DateTime? createPeriode { get; set; }
        public DateTime? inputPeriode { get; set; }
    }
}
