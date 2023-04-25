using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstTypeClassification
{
    public Guid TypeClassificationId { get; set; }

    public string? TypeClassificationCode { get; set; }

    public string? TypeClassificationName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<MstTypeClassificationLog> MstTypeClassificationLogs { get; } = new List<MstTypeClassificationLog>();

    public virtual ICollection<TrxClassification> TrxClassifications { get; } = new List<TrxClassification>();
}
