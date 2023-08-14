using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstTemplateSettingDetail
{
    public Guid TemplateSettingDetailId { get; set; }

    public Guid TemplateSettingId { get; set; }

    public string VariableName { get; set; } = null!;

    public string VariableType { get; set; } = null!;

    public string VariableData { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual MstTemplateSetting TemplateSetting { get; set; } = null!;
}
