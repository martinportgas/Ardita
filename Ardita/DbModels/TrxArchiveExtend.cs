using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class TrxArchiveExtend
{
    public Guid ArchiveExtendId { get; set; }

    public Guid ArchiveUnitId { get; set; }

    public string ExtendCode { get; set; } = null!;

    public string ExtendName { get; set; } = null!;

    public string? Description { get; set; }

    public int ApproveLevel { get; set; }

    public int ApproveMax { get; set; }

    public string DocumentCode { get; set; } = null!;

    public long StatusId { get; set; }

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchiveActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit ArchiveUnit { get; set; } = null!;

    public virtual MstStatus Status { get; set; } = null!;

    public virtual ICollection<TrxArchiveExtendDetail> TrxArchiveExtendDetails { get; } = new List<TrxArchiveExtendDetail>();
}
