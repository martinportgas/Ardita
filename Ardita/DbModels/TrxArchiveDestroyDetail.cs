using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class TrxArchiveDestroyDetail
{
    public Guid ArchiveDestroyDetailId { get; set; }

    public Guid ArchiveDestroyId { get; set; }

    public Guid ArchiveId { get; set; }

    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual TrxArchiveDestroy ArchiveDestroy { get; set; } = null!;
}
