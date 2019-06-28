using DevTask.EvenBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTask.EvenBus.DomainEvents
{
    public class KYCVerifcationReceveidEvent : KYCEvent
    {
        public string VerificationResponse { get; set; }

    }
}
