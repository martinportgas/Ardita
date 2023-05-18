using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxMediaStorageInActive
{
    public Guid MediaStorageInActiveId { get; set; }

    public Guid RowId { get; set; }

    public Guid SubSubjectClassificationId { get; set; }

    public Guid TypeStorageId { get; set; }

    public string MediaStorageInActiveCode { get; set; } = null!;

    public string MediaStorageInActiveName { get; set; } = null!;

    public string ArchiveYear { get; set; } = null!;

    public int TotalVolume { get; set; }

    public int DifferenceVolume { get; set; }

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public long StatusId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxRow Row { get; set; } = null!;

    public virtual MstStatus Status { get; set; } = null!;

    public virtual TrxSubSubjectClassification SubSubjectClassification { get; set; } = null!;

    public virtual ICollection<TrxMediaStorageInActiveDetail> TrxMediaStorageInActiveDetails { get; } = new List<TrxMediaStorageInActiveDetail>();

    public virtual TrxTypeStorage TypeStorage { get; set; } = null!;
}
