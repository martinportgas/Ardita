using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstMenu
{
    public Guid MenuId { get; set; }

    public string Name { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<MstSubmenu> MstSubmenus { get; } = new List<MstSubmenu>();
}
