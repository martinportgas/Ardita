using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class MstPage
{
    public Guid PageId { get; set; }

    public Guid SubmenuId { get; set; }

    public string Name { get; set; } = null!;

    public string? Path { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<IdxRolePage> IdxRolePages { get; } = new List<IdxRolePage>();

    public virtual ICollection<MstPageDetail> MstPageDetails { get; } = new List<MstPageDetail>();

    public virtual MstSubmenu Submenu { get; set; } = null!;
}
