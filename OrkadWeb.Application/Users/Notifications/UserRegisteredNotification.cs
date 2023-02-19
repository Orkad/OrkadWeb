using MediatR;
using OrkadWeb.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Users.Notifications
{
    public class UserRegisteredNotification : INotification
    {
        public string UserName { get; set; }
    }
}
