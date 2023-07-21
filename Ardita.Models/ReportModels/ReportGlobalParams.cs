﻿using System;
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
        public bool? status { get; set; }
        public DateTime? startDate { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDate { get; set; } = DateTime.Now;
        public DateTime? startDateCreated { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateCreated { get; set; } = DateTime.Now;
        public DateTime? startDateDestroy { get; set; } = DateTime.Parse("1900-01-01");
        public DateTime? endDateDestroy { get; set; } = DateTime.Now;
    }
}
