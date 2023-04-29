using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveExtendDetail
{
    public Guid ArchiveExtendDetailId { get; set; }

    public Guid ArchiveExtendId { get; set; }

    public Guid ArchiveId { get; set; }

    public int? RetentionBefore { get; set; }

    public int? RetensionAfter { get; set; }

    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual TrxArchiveExtend ArchiveExtend { get; set; } = null!;
}
