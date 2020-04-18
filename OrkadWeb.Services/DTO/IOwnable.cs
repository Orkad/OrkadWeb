using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Services.DTO
{
    public interface IOwnable
    {
        public int OwnerId { get; set; }
    }
}
