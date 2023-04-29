namespace Ardita.Models.ViewModels.SubSubjectClasscification;

public class SubSubjectClasscificationViewModel
{
    public Guid SubSubjectClassificationId { get; set; }
    public Guid? CreatorId { get; set; }
    public string? SubSubjectClassificationName { get; set; }
    public string? SubSubjectClassificationCode { get; set; }
    public string? CreatorName { get; set; }
    public int? RetentionActive { get; set; }
    public int? RetentionInactive { get; set; }
}
