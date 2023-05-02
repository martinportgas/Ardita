using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstStatus
{
    public long StatusId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<TrxApproval> TrxApprovals { get; } = new List<TrxApproval>();

    public virtual ICollection<TrxArchiveDestroy> TrxArchiveDestroys { get; } = new List<TrxArchiveDestroy>();

    public virtual ICollection<TrxArchiveExtend> TrxArchiveExtends { get; } = new List<TrxArchiveExtend>();

    public virtual ICollection<TrxArchiveMovement> TrxArchiveMovements { get; } = new List<TrxArchiveMovement>();

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();
}
