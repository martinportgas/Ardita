using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class TrxPermissionClassification
{
    public Guid PermissionClassificationId { get; set; }

    public Guid SubSubjectClassificationId { get; set; }

    public Guid PositionId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstPosition Position { get; set; } = null!;

    public virtual TrxSubSubjectClassification SubSubjectClassification { get; set; } = null!;
}
