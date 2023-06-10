using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxMediaStorage
{
    public Guid MediaStorageId { get; set; }

    public Guid SubjectClassificationId { get; set; }

    public Guid TypeStorageId { get; set; }

    public Guid RowId { get; set; }

    public string MediaStorageCode { get; set; } = null!;

    public string? MediaStorageName { get; set; }

    public string ArchiveYear { get; set; } = null!;

    public int TotalVolume { get; set; }

    public int DifferenceVolume { get; set; }

    public bool IsActive { get; set; }

    public long StatusId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxRow Row { get; set; } = null!;

    public virtual MstStatus Status { get; set; } = null!;

    public virtual TrxSubjectClassification SubjectClassification { get; set; } = null!;

    public virtual ICollection<TrxMediaStorageDetail> TrxMediaStorageDetails { get; } = new List<TrxMediaStorageDetail>();

    public virtual TrxTypeStorage TypeStorage { get; set; } = null!;
}
