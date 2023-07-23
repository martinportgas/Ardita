using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ReportModels
{
    public class ReportGlobalParams
    {
        public Guid companyId { get; set; }
        public Guid archiveUnitId { get; set; }
        public Guid archiveUnitFromId { get; set; }
        public Guid roomId { get; set; }
        public Guid rackId { get; set; }
        public Guid levelId { get; set; }
        public Guid rowId { get; set; }
        public Guid gmdId { get; set; }
        public Guid typeStorageId { get; set; }
        public Guid creatorId { get; set; }
        public Guid archiveOwnerId { get; set; }
        public Guid classificationId { get; set; }
        public Guid subjectClassificationId { get; set; }
        public Guid borrowerId { get; set; }
        public Guid PIC { get; set; }
        public Guid senderId { get; set; }
        public Guid receiverId { get; set; }
        public bool? status { get; set; }
        public long statusId { get; set; }
        public DateTime? startDate { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDate { get; set; } = DateTime.Now;
        public DateTime? startDateCreated { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateCreated { get; set; } = DateTime.Now;
        public DateTime? startDateDestroy { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateDestroy { get; set; } = DateTime.Now;
        public DateTime? startDateRent { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateRent { get; set; } = DateTime.Now;
        public DateTime? startDateReceive { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateReceive { get; set; } = DateTime.Now;
        public DateTime? startDateMove { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateMove { get; set; } = DateTime.Now;
        public DateTime? startDateUse { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateUse { get; set; } = DateTime.Now;
        public Guid sender { get; set; }
        public Guid receiver { get; set; }
    }
}
