using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchive
{
    public Guid ArchiveId { get; set; }

    public Guid? GmdId { get; set; }

    public Guid? SubSubjectClassificationId { get; set; }

    public Guid? SecurityClassificationId { get; set; }

    public Guid? CreatorId { get; set; }

    public string? TypeSender { get; set; }

    public string? Keyword { get; set; }

    public string? TitleArchive { get; set; }

    public string? TypeArchive { get; set; }

    public DateTime? CreatedDateArchive { get; set; }

    public int? ActiveRetention { get; set; }

    public int? InactiveRetention { get; set; }

    public int? Volume { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCreator? Creator { get; set; }

    public virtual MstGmd? Gmd { get; set; }

    public virtual MstSecurityClassification? SecurityClassification { get; set; }

    public virtual TrxSubSubjectClassification? SubSubjectClassification { get; set; }

    public virtual ICollection<TrxArchiveMovement> TrxArchiveMovements { get; } = new List<TrxArchiveMovement>();

    public virtual ICollection<TrxMediaStorageDetail> TrxMediaStorageDetails { get; } = new List<TrxMediaStorageDetail>();
}
