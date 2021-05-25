using Atlantico.CrossCutting.Massages.Interfaces;
using Atlantico.CrossCutting.Messages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Atlantico.WebApi.Controllers
{
    public class MainController : ControllerBase
    {

        private readonly INotificator _notification;

        protected MainController(INotificator notification)
        {
            _notification = notification;
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(error: new
            {
                errors = _notification.GetNotifications().Select(n => n.Message)
            });
        }

        protected bool IsValidOperation()
        {
            return !_notification.HasNotification();
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                NotificationErrorModelInvalid(modelState);
            }

            return CustomResponse();
        }

        protected void NotificationErrorModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotificationError(errorMsg);
            }
        }

        protected void NotificationError(string message)
        {
            _notification.Handle(new Notification(message));
        }
    }
}