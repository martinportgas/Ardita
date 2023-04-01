using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstSubmenu
{
    public Guid SubmenuId { get; set; }

    public Guid MenuId { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public int Sort { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual MstMenu Menu { get; set; } = null!;

    public virtual ICollection<MstPage> MstPages { get; } = new List<MstPage>();
}
