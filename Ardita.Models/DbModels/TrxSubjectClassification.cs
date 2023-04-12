using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxSubjectClassification
{
    public Guid SubjectClassificationId { get; set; }

    public Guid? TypeClassificationId { get; set; }

    public string? SubjectClassificationCode { get; set; }

    public string? SubjectClassificationName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; } = new List<TrxSubSubjectClassification>();

    public virtual MstTypeClassification? TypeClassification { get; set; }
}
