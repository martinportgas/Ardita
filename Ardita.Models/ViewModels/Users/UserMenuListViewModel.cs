namespace Ardita.Models.ViewModels.Users;

public class UserMenuListViewModel
{
    public Guid MenuId { get; set; }
    public string MenuName { get; set; }
    public string MenuPath { get; set; }
    public Guid SubMenuId { get; set; }
    public string SubMenuName { get; set; }
    public string SubMenuPath { get; set; }
    public int SubMenuSort { get; set; }
    public int? MenuSort { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; }
    public string PagePath { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}
