using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.EventDtos
{
    public class AllEventsDtos
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public int MaxAttendance { get; set; }
        public string EventStatus { get; set; }
        public bool PaymentRequired { get; set; }

        #region Relations
        public int OrganizerId { get; set; }
        public string OrganizerName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        
        #endregion
    }
}
