using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstPage
{
    public Guid PageId { get; set; }

    public Guid SubmenuId { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<MstRolePage> MstRolePages { get; } = new List<MstRolePage>();

    public virtual MstSubmenu Submenu { get; set; } = null!;
}
