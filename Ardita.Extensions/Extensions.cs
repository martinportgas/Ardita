using Ardita.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Azure.Core.HttpHeader;

namespace Ardita.Extensions
{
    public static class Extensions
    {
        public static string IsSelected(this IHtmlHelper html, string area = null, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(area))
                area = currentArea;

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return area == currentArea && controller == currentController ?
                cssClass : String.Empty;
        }
        public static FormModel Form(this IHtmlHelper html)
        {
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            string FormAction = GlobalConst.Save;

            string LastBreadcrumb = GlobalConst.Create;

            if (currentAction == GlobalConst.Update)
            {
                LastBreadcrumb = GlobalConst.Update;
            }

            if (currentAction == GlobalConst.Detail)
            {
                LastBreadcrumb = GlobalConst.Detail;
            }

            if (currentAction == GlobalConst.Remove)
            {
                FormAction = GlobalConst.Delete;
                LastBreadcrumb = GlobalConst.Delete;
            }

            if (currentAction == GlobalConst.Preview)
            {
                FormAction = GlobalConst.Submit;
                LastBreadcrumb = GlobalConst.Submit;
            }

            if (currentAction == GlobalConst.Approval)
            {
                FormAction = GlobalConst.SubmitApproval;
                LastBreadcrumb = GlobalConst.Approval;
            }

            var isInput = currentAction == GlobalConst.Add || currentAction == GlobalConst.Update || currentAction == GlobalConst.Remove;
            var isSubmitForm = isInput || currentAction == GlobalConst.Remove || currentAction == GlobalConst.Preview || currentAction == GlobalConst.Approval;

            FormModel model = new FormModel();
            model.CurrentArea = currentArea;
            model.CurrentController = currentController;
            model.CurrentAction = currentAction;
            model.FormAction = FormAction;
            model.LastBreadcrumb = LastBreadcrumb;
            model.isInput = isInput;
            model.isSubmitForm = isSubmitForm;
            model.SubmitText = currentAction == GlobalConst.Remove ? string.Concat(GlobalConst.IconDelete, GlobalConst.Delete) : string.Concat(GlobalConst.IconSubmit, GlobalConst.Submit);
            model.SaveText = currentAction == GlobalConst.Remove ? string.Concat(GlobalConst.IconDelete, GlobalConst.Delete) : string.Concat(GlobalConst.IconSave, GlobalConst.Save);
            model.ApprovalText = string.Concat(GlobalConst.IconApprove, GlobalConst.Approve);
            model.RejectText = string.Concat(GlobalConst.IconReject, GlobalConst.Reject);
            model.CancelText = string.Concat(GlobalConst.IconBack, GlobalConst.Cancel);
            model.BackText = string.Concat(GlobalConst.IconBack, GlobalConst.Back);
            model.ButtonSubmitClass = currentAction == GlobalConst.Remove ? GlobalConst.BtnDanger : GlobalConst.BtnPrimary;

            return model;
        }
        public static string ToCleanNameOf(this string nameOf)
        {
            if (nameOf.Length > 0)
            {
                string prefix = nameOf.Substring(0, 3);

                if (prefix.ToLower() == "trx")
                    return nameOf.Replace("Trx", string.Empty);

                if (prefix.ToLower() == "mst")
                    return nameOf.Replace("Mst", string.Empty);

                if (prefix.ToLower() == "idx")
                    return nameOf.Replace("Idx", string.Empty);
            }
            return nameOf;
        }
    }
}
