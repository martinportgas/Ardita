using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstGeneralSetting
{
    public Guid GeneralSettingsId { get; set; }

    public string SiteLogo { get; set; } = null!;

    public string CompanyLogo { get; set; } = null!;

    public string FavIcon { get; set; } = null!;

    public string AplicationTitle { get; set; } = null!;

    public string TimeAndZone { get; set; } = null!;

    public string Footer { get; set; } = null!;

    public string LicenseKey { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<IdxGeneralSettingsFormatFile> IdxGeneralSettingsFormatFiles { get; } = new List<IdxGeneralSettingsFormatFile>();
}
