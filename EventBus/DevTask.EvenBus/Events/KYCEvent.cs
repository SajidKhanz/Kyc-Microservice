using System;
using System.Collections.Generic;
using System.Text;

namespace DevTask.EvenBus.Events
{
    public class KYCEvent
    {
        public int EventId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedDate { get; set; }   

    }
}
