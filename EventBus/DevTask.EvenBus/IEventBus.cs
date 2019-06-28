using DevTask.EvenBus.Events;
using System;

namespace DevTask.EvenBus
{
   public interface IEventBus
    {
        void Publish(KYCEvent kYCEvent);
        void Subscribe(string kYCEvent, Type handler);
        
    }
}
