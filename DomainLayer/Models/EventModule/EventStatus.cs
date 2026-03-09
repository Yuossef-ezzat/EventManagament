using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.EventModule
{
    public enum EventStatus
    {
        Scheduled = 1, 
        Completed = 2, 
        Canceled = 3
    }
}
