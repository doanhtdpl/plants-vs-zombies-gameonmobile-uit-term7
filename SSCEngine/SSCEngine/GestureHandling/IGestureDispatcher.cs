using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling
{
    public interface IGestureDispatcher
    {
        void AddTarget<GestureEvent>(IGestureTarget<GestureEvent> gTarget) where GestureEvent : IGestureEvent;
        void RemoveTarget<GestureEvent>(IGestureTarget<GestureEvent> gTarget) where GestureEvent : IGestureEvent;

        void Dispatch<GestureEvent>(GestureEvent gEvent) where GestureEvent : IGestureEvent;

        bool Enabled { get; set; }
    }
}