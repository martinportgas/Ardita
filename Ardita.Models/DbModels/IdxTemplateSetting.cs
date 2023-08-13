using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class IdxTemplateSetting
{
    public Guid IdxTemplateSettingId { get; set; }

    public string TemplateType { get; set; } = null!;

    public string TemplateName { get; set; } = null!;

    public string SourceData { get; set; } = null!;

    public string Path { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
