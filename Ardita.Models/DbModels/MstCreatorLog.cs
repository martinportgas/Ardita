using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstCreatorLog
{
    public Guid CreatorIdLog { get; set; }

    public Guid CreatorId { get; set; }

    public string? CreatorCode { get; set; }

    public string? CreatorType { get; set; }

    public string? CreatorName { get; set; }

    public bool? IsActive { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCreator Creator { get; set; } = null!;
}
