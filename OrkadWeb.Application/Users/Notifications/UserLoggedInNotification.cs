using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Users.Notifications
{
    public class UserLoggedInNotification : INotification
    {
        public string UserName { get; set; }
    }
}
