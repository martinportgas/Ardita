using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveOutIndicator
{
    public Guid ArchiveOutIndicatorId { get; set; }

    public Guid MediaStorageId { get; set; }

    public Guid ArchiveId { get; set; }

    public string UsedBy { get; set; } = null!;

    public DateTime UsedDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime? ReturnDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;
}
