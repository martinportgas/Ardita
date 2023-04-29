using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxMediaStorageDetail
{
    public Guid MediaStorageDetailId { get; set; }

    public Guid MediaStorageId { get; set; }

    public Guid ArchiveId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual TrxMediaStorage MediaStorage { get; set; } = null!;
}
