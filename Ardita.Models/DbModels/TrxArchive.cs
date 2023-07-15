using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchive
{
    public Guid ArchiveId { get; set; }

    public Guid GmdId { get; set; }

    public Guid? GmdDetailId { get; set; }

    public Guid SubSubjectClassificationId { get; set; }

    public Guid SecurityClassificationId { get; set; }

    public Guid CreatorId { get; set; }

    public Guid ArchiveOwnerId { get; set; }

    public Guid ArchiveTypeId { get; set; }

    public string TypeSender { get; set; } = null!;

    public string Keyword { get; set; } = null!;

    public string? ArchiveCode { get; set; }

    public string? DocumentNo { get; set; }

    public string TitleArchive { get; set; } = null!;

    public DateTime CreatedDateArchive { get; set; }

    public int ActiveRetention { get; set; }

    public int InactiveRetention { get; set; }

    public int Volume { get; set; }

    public string? Description { get; set; }

    public string? ArchiveDescription { get; set; }

    public long StatusId { get; set; }

    public bool? IsUsed { get; set; }

    public string? IsUsedBy { get; set; }

    public DateTime? IsUsedDate { get; set; }

    public bool IsActive { get; set; }

    public bool? IsArchiveActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstArchiveOwner ArchiveOwner { get; set; } = null!;

    public virtual MstArchiveType ArchiveType { get; set; } = null!;

    public virtual MstCreator Creator { get; set; } = null!;

    public virtual MstGmd Gmd { get; set; } = null!;

    public virtual MstGmdDetail? GmdDetail { get; set; }

    public virtual MstSecurityClassification SecurityClassification { get; set; } = null!;

    public virtual MstStatus Status { get; set; } = null!;

    public virtual TrxSubSubjectClassification SubSubjectClassification { get; set; } = null!;

    public virtual ICollection<TrxArchiveDestroyDetail> TrxArchiveDestroyDetails { get; } = new List<TrxArchiveDestroyDetail>();

    public virtual ICollection<TrxArchiveExtendDetail> TrxArchiveExtendDetails { get; } = new List<TrxArchiveExtendDetail>();

    public virtual ICollection<TrxArchiveMovementDetail> TrxArchiveMovementDetails { get; } = new List<TrxArchiveMovementDetail>();

    public virtual ICollection<TrxArchiveOutIndicator> TrxArchiveOutIndicators { get; } = new List<TrxArchiveOutIndicator>();

    public virtual ICollection<TrxArchiveRentDetail> TrxArchiveRentDetails { get; } = new List<TrxArchiveRentDetail>();

    public virtual ICollection<TrxFileArchiveDetail> TrxFileArchiveDetails { get; } = new List<TrxFileArchiveDetail>();

    public virtual ICollection<TrxMediaStorageDetail> TrxMediaStorageDetails { get; } = new List<TrxMediaStorageDetail>();

    public virtual ICollection<TrxMediaStorageInActiveDetail> TrxMediaStorageInActiveDetails { get; } = new List<TrxMediaStorageInActiveDetail>();
}
