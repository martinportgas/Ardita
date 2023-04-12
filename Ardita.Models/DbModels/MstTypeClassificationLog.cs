using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstTypeClassificationLog
{
    public Guid TypeClassificationIdLog { get; set; }

    public Guid TypeClassificationId { get; set; }

    public string? TypeClassificationName { get; set; }

    public bool? IsActive { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstTypeClassification TypeClassification { get; set; } = null!;
}
