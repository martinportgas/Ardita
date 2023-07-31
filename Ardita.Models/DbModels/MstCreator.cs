using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstCreator
{
    public Guid CreatorId { get; set; }

    public Guid ArchiveUnitId { get; set; }

    public string CreatorCode { get; set; } = null!;

    public string CreatorName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit ArchiveUnit { get; set; } = null!;

    public virtual ICollection<IdxUserRole> IdxUserRoles { get; } = new List<IdxUserRole>();

    public virtual ICollection<MstCreatorLog> MstCreatorLogs { get; } = new List<MstCreatorLog>();

    public virtual ICollection<MstUser> MstUsers { get; } = new List<MstUser>();

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();

    public virtual ICollection<TrxClassification> TrxClassifications { get; } = new List<TrxClassification>();

    public virtual ICollection<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; } = new List<TrxSubSubjectClassification>();
}
