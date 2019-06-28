using DevTask.EvenBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTask.EvenBus.DomainEvents
{
    public class KYCEventHandlerFactory
    {
        private Dictionary<string, Type> Subsriptions { get; set; }

        public KYCEventHandlerFactory()
        {
            Subsriptions = new Dictionary<string, Type>();
        }

        public IKYCEventHandler GetEventHandlerByEvent(string eventName) 
        {
            if (Subsriptions.TryGetValue(eventName, out Type result))
            {
                return (IKYCEventHandler)Activator.CreateInstance(result);
            }
            else
            {
                throw new Exception("Could not find the handler");
            }
          
        }


        public void AddSubscription(string key, Type type)
        {
            if (typeof(IKYCEventHandler).IsAssignableFrom(type))
            {
                Subsriptions.Add(key, type);
            }
            else
            {
                throw new Exception("Invalid type is subscribed!");
            }

        }


    }
}
