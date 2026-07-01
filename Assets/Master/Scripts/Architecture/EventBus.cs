using System;
using System.Collections.Generic;

namespace RPGTest.Architecture 
{
    public static class EventBus<T>
    {
        private static readonly List<Action<T>> listeners = new List<Action<T>>();

        public static void Subscribe(Action<T> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public static void Unsubscribe(Action<T> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public static void Publish(T eventData)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i]?.Invoke(eventData);
            }
        }
    }
}