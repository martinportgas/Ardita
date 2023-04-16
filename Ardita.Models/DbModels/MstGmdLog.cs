using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstGmdLog
{
    public Guid GmdIdLog { get; set; }

    public Guid GmdId { get; set; }

    public string? GmdCode { get; set; }

    public string? GmdName { get; set; }

    public bool? IsActive { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstGmd Gmd { get; set; } = null!;
}
