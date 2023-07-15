using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveRentDetail
{
    public Guid ArchiveRentDetailId { get; set; }

    public Guid TrxArchiveRentId { get; set; }

    public Guid ArchiveId { get; set; }

    public Guid MediaStorageInActiveId { get; set; }

    public int Sort { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual TrxMediaStorageInActive MediaStorageInActive { get; set; } = null!;

    public virtual TrxArchiveRent TrxArchiveRent { get; set; } = null!;
}
