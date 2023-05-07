using Ardita.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Azure.Core.HttpHeader;

namespace Ardita.Globals
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

            string FormAction = Const.Save;

            string LastBreadcrumb = Const.Create;

            if (currentAction == Const.Update)
            {
                LastBreadcrumb = Const.Update;
            }

            if (currentAction == Const.Detail)
            {
                LastBreadcrumb = Const.Detail;
            }

            if (currentAction == Const.Remove)
            {
                FormAction = Const.Delete;
                LastBreadcrumb = Const.Delete;
            }

            if (currentAction == Const.Preview)
            {
                FormAction = Const.Submit;
                LastBreadcrumb = Const.Submit;
            }

            if (currentAction == Const.Approval)
            {
                FormAction = Const.SubmitApproval;
                LastBreadcrumb = Const.Approval;
            }

            var isInput = currentAction == Const.Add || currentAction == Const.Update;
            var isSubmitForm = isInput || currentAction == Const.Remove || currentAction == Const.Preview|| currentAction == Const.Approval;

            FormModel model = new FormModel();
            model.CurrentArea = currentArea;
            model.CurrentController = currentController;
            model.CurrentAction = currentAction;
            model.FormAction = FormAction;
            model.LastBreadcrumb = LastBreadcrumb;
            model.isInput = isInput;
            model.isSubmitForm = isSubmitForm;
            model.SubmitText = currentAction == Const.Remove ? Const.Delete : Const.Submit;
            model.SaveText = currentAction == Const.Remove ? Const.Delete : Const.Save;
            model.ApprovalText = currentAction == Const.Remove ? Const.Delete : Const.Approve;

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
