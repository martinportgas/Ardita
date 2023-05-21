using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveDestroy
{
    public Guid ArchiveDestroyId { get; set; }

    public string? DestroyCode { get; set; }

    public string? DestroyName { get; set; }

    public string? Description { get; set; }

    public int? ApproveLevel { get; set; }

    public int ApproveMax { get; set; }

    public string? DocumentCode { get; set; }

    public DateTime? DestroySchedule { get; set; }

    public long StatusId { get; set; }

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchiveActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstStatus Status { get; set; } = null!;

    public virtual ICollection<TrxArchiveDestroyDetail> TrxArchiveDestroyDetails { get; } = new List<TrxArchiveDestroyDetail>();
}
