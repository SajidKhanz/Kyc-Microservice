using System;
using System.Collections.Generic;
using System.Text;

namespace DevTask.EvenBus.Events
{
    public interface IKYCEventHandler {
        void Handle(string message);
    }
}
