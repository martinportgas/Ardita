using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstSecurityClassification
{
    public Guid SecurityClassificationId { get; set; }

    public string SecurityClassificationCode { get; set; } = null!;

    public string SecurityClassificationName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<MstSecurityClassificationLog> MstSecurityClassificationLogs { get; } = new List<MstSecurityClassificationLog>();

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();

    public virtual ICollection<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; } = new List<TrxSubSubjectClassification>();
}
