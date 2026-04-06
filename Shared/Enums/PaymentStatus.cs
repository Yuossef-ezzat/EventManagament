using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PaymentModule
{
    public enum PaymentStatus
    {
        Success = 1,
        Failed = 2,
        pending = 3,
        Required = 4,
        NorRequired = 5
    }
}
