using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstCreator
{
    public Guid CreatorId { get; set; }

    public string? CreatorCode { get; set; }

    public string? CreatorType { get; set; }

    public string? CreatorName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<MstCreatorLog> MstCreatorLogs { get; } = new List<MstCreatorLog>();

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();

    public virtual ICollection<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; } = new List<TrxSubSubjectClassification>();
}
