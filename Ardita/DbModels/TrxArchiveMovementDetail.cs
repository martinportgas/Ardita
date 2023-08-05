using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class TrxArchiveMovementDetail
{
    public Guid ArchiveMovementDetailId { get; set; }

    public Guid ArchiveMovementId { get; set; }

    public Guid ArchiveId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual TrxArchiveMovement ArchiveMovement { get; set; } = null!;
}
