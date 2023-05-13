using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxSubSubjectClassification
{
    public Guid SubSubjectClassificationId { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? SubjectClassificationId { get; set; }

    public Guid? SecurityClassificationId { get; set; }

    public string? SubSubjectClassificationName { get; set; }

    public string? SubSubjectClassificationCode { get; set; }

    public int? RetentionActive { get; set; }

    public int? RetentionInactive { get; set; }

    public string? BasicInformation { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCreator? Creator { get; set; }

    public virtual MstSecurityClassification? SecurityClassification { get; set; }

    public virtual TrxSubjectClassification? SubjectClassification { get; set; }

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();

    public virtual ICollection<TrxMediaStorageInActive> TrxMediaStorageInActives { get; } = new List<TrxMediaStorageInActive>();

    public virtual ICollection<TrxPermissionClassification> TrxPermissionClassifications { get; } = new List<TrxPermissionClassification>();
}
