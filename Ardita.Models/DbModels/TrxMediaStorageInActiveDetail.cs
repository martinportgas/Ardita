using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxMediaStorageInActiveDetail
{
    public Guid MediaStorageInActiveDetailId { get; set; }

    public Guid MediaStorageInActiveId { get; set; }

    public int Sort { get; set; }

    public Guid? SubTypeStorageId { get; set; }

    public Guid ArchiveId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual TrxMediaStorageInActive MediaStorageInActive { get; set; } = null!;

    public virtual MstSubTypeStorage? SubTypeStorage { get; set; }
}
