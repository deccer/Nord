﻿using System;
using System.Collections.Generic;

namespace MonoGame.WpfCore.MonoGameControls
{
    public class MonoGameServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services;

        public MonoGameServiceProvider()
        {
            _services = new Dictionary<Type, object>();
        }

        public void AddService(Type type, object provider)
        {
            _services.Add(type, provider);
        }

        public object GetService(Type type) => _services.TryGetValue(type, out var service)
            ? service
            : null;

        public void RemoveService(Type type)
        {
            _services.Remove(type);
        }

        public void AddService<T>(T service)
        {
            AddService(typeof(T), service);
        }

        public T GetService<T>() where T : class
        {
            var service = GetService(typeof(T));
            return (T) service;
        }
    }
}
