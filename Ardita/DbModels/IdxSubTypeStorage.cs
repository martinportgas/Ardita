using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class IdxSubTypeStorage
{
    public Guid SubTypeStorageDetailId { get; set; }

    public Guid SubTypeStorageId { get; set; }

    public Guid TypeStorageId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public virtual MstSubTypeStorage SubTypeStorage { get; set; } = null!;

    public virtual TrxTypeStorage TypeStorage { get; set; } = null!;
}
