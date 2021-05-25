using Atlantico.CrossCutting.Messages.Models;
using System.Collections.Generic;

namespace Atlantico.CrossCutting.Massages.Interfaces
{
    public interface INotificator
    {

        public void Handle(Notification notification);

        public List<Notification> GetNotifications();

        public bool HasNotification();

        public void notify(string message);
    }
}