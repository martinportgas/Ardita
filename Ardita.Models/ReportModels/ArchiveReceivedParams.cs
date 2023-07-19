using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ReportModels
{
    public class ArchiveReceivedParams
    {
        public Guid? companyId { get; set; }
        public Guid? archiveUnit { get; set; }
        public Guid? statusId { get; set; }
        public Guid? gmdId { get; set; }
        public Guid? ownerId { get; set; }
        public Guid? senderId { get; set; }
        public Guid? receiverId { get; set; }
        public Guid? clasificationId { get; set; }
        public Guid? subjectClasification { get; set; }
        public DateTime? createPeriode { get; set; }
        public DateTime? receivePeriode { get; set; }
    }
}
