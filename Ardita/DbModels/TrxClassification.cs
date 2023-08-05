using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class TrxClassification
{
    public Guid ClassificationId { get; set; }

    public Guid CreatorId { get; set; }

    public Guid TypeClassificationId { get; set; }

    public string ClassificationCode { get; set; } = null!;

    public string ClassificationName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCreator Creator { get; set; } = null!;

    public virtual ICollection<TrxSubjectClassification> TrxSubjectClassifications { get; } = new List<TrxSubjectClassification>();

    public virtual MstTypeClassification TypeClassification { get; set; } = null!;
}
