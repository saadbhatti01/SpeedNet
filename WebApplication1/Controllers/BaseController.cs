using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {


        }

        public virtual void Notify(NotificationMessage message)
        {
            if (message.IsRedirectMessage)
            {
                TempData["NotificationMessage"] = message;
            }
            if (message.IsViewMessage)
            {
                ViewBag.NotificationMessage = message;
            }
        }

        public void Notify(string Type, string Title, string Description, bool IsAjaxMessage = true, bool IsViewMessage = true, bool IsRedirectMessage = false)
        {
            Notify(new NotificationMessage
            {
                MessageType = Type,
                Title = Title,
                Description = Description,
                IsAjaxMessage = IsAjaxMessage,
                IsViewMessage = IsViewMessage,
                IsRedirectMessage = IsRedirectMessage
            });
        }



        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        }
    }
}