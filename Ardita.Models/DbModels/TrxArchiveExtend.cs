using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveExtend
{
    public Guid ArchiveExtendId { get; set; }

    public string? ExtendCode { get; set; }

    public string? ExtendName { get; set; }

    public string? Description { get; set; }

    public int? ApproveLevel { get; set; }

    public int? ApproveMax { get; set; }

    public long StatusId { get; set; }

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstStatus Status { get; set; } = null!;

    public virtual ICollection<TrxArchiveExtendDetail> TrxArchiveExtendDetails { get; } = new List<TrxArchiveExtendDetail>();
}
