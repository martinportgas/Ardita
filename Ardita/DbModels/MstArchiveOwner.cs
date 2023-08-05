using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class MstArchiveOwner
{
    public Guid ArchiveOwnerId { get; set; }

    public string ArchiveOwnerCode { get; set; } = null!;

    public string ArchiveOwnerName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();
}
