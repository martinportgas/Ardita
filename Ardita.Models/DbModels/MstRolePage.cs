using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstRolePage
{
    public Guid RolePageId { get; set; }

    public Guid RoleId { get; set; }

    public Guid PageId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual MstPage Page { get; set; } = null!;

    public virtual MstRole Role { get; set; } = null!;
}
