using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxClassification
{
    public Guid ClassificationId { get; set; }

    public Guid? TypeClassificationId { get; set; }

    public string? ClassificationCode { get; set; }

    public string? ClassificationName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; } = new List<TrxSubSubjectClassification>();

    public virtual ICollection<TrxSubjectClassification> TrxSubjectClassifications { get; } = new List<TrxSubjectClassification>();

    public virtual MstTypeClassification? TypeClassification { get; set; }
}
