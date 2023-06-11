using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstSubTypeStorageDetail
{
    public Guid SubTypeStorageDetailId { get; set; }

    public Guid SubTypeStorageId { get; set; }

    public Guid GmdDetailId { get; set; }

    public int Size { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual MstGmdDetail GmdDetail { get; set; } = null!;

    public virtual MstSubTypeStorage SubTypeStorage { get; set; } = null!;
}
