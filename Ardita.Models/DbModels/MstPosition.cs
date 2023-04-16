using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstPosition
{
    public Guid PositionId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<MstEmployee> MstEmployees { get; } = new List<MstEmployee>();

    public virtual ICollection<TrxPermissionClassification> TrxPermissionClassifications { get; } = new List<TrxPermissionClassification>();
}
