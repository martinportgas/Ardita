using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class MstSecurityClassificationLog
{
    public Guid SecurityClassificationIdLog { get; set; }

    public Guid SecurityClassificationId { get; set; }

    public string? SecurityClassificationCode { get; set; }

    public string? SecurityClassificationName { get; set; }

    public bool? IsActive { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstSecurityClassification SecurityClassification { get; set; } = null!;
}
