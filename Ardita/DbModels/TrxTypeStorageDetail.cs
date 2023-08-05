using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class TrxTypeStorageDetail
{
    public Guid TypeStorageDetailId { get; set; }

    public Guid TypeStorageId { get; set; }

    public Guid GmdDetailId { get; set; }

    public int Size { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual MstGmdDetail GmdDetail { get; set; } = null!;

    public virtual TrxTypeStorage TypeStorage { get; set; } = null!;
}
