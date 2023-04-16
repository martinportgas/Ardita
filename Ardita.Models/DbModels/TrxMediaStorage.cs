using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxMediaStorage
{
    public Guid MediaStorageId { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? TypeStorageId { get; set; }

    public Guid? GmdId { get; set; }

    public Guid? RoomId { get; set; }

    public Guid? RackId { get; set; }

    public Guid? LevelId { get; set; }

    public Guid? RowId { get; set; }

    public string? MediaStorageCode { get; set; }

    public string? MediaStorageName { get; set; }

    public int? TotalVolume { get; set; }

    public int? DifferenceVolume { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCreator? Creator { get; set; }

    public virtual MstGmd? Gmd { get; set; }

    public virtual TrxLevel? Level { get; set; }

    public virtual TrxRack? Rack { get; set; }

    public virtual TrxRoom? Room { get; set; }

    public virtual TrxRow? Row { get; set; }

    public virtual ICollection<TrxMediaStorageDetail> TrxMediaStorageDetails { get; } = new List<TrxMediaStorageDetail>();

    public virtual TrxTypeStorage? TypeStorage { get; set; }
}
