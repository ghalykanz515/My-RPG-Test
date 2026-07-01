using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest.Architecture 
{
    public class ServiceLocator
    {
        private static ServiceLocator current;
        public static ServiceLocator Current
        {
            get
            {
                if (current == null) Initialize();
                return current;
            }
        }
    
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
    
        public static void Initialize()
        {
            if (current != null) return;
            current = new ServiceLocator();
        }
    
        public void Register<T>(T service)
        {
            Type type = typeof(T);
            if (services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Service bertipe {type.Name} sudah terdaftar sebelumnya.");
                return;
            }
            services.Add(type, service);
        }
    
        public T Get<T>()
        {
            Type type = typeof(T);
            if (!services.TryGetValue(type, out object service))
            {
                Debug.LogError($"[ServiceLocator] Service bertipe {type.Name} belum terdaftar!");
                return default;
            }
            return (T)service;
        }
    
        public bool Contains<T>()
        {
            return services.ContainsKey(typeof(T));
        }
    
        public void Unregister<T>()
        {
            Type type = typeof(T);
            if (services.ContainsKey(type))
            {
                services.Remove(type);
            }
        }
    }
}