using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class IdxUserArchiveUnit
{
    public Guid UserArchiveUnitId { get; set; }

    public Guid UserId { get; set; }

    public Guid ArchiveUnitId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public virtual TrxArchiveUnit ArchiveUnit { get; set; } = null!;

    public virtual MstUser User { get; set; } = null!;
}
