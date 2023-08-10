using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class IdxGeneralSettingsFormatFile
{
    public Guid GeneralSettingsFormatFileId { get; set; }

    public Guid GeneralSettingsId { get; set; }

    public string FormatFileName { get; set; } = null!;

    public virtual MstGeneralSetting GeneralSettings { get; set; } = null!;
}
