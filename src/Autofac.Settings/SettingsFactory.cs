using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Autofac.Settings {
    public class SettingsFactory : ISettingsFactory {
        readonly IEnumerable<ISettingsProvider> _providers;

        public SettingsFactory(IEnumerable<ISettingsProvider> providers) {
            _providers = providers;
        }

        public object Create(Type type) {
            var obj = Activator.CreateInstance(type);

            var propertiesMissed = PopulateProperties(type, obj);

            if (propertiesMissed.Any()) {
                var message = String.Format("{0} is missing the following properties in configuration: {1}", type.Name, String.Join(",", propertiesMissed));
                throw new ConfigurationErrorsException(message);
            }

            return obj;
        }

        private List<string> PopulateProperties(Type type, object obj) {
            var propertiesMissed = new List<string>();

            var props = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite);

            foreach (var prop in props) {
                var value = _providers
                    .Select(p => p.ProvideValueFor(type, prop.Name))
                    .FirstOrDefault(p => p != null);
                if (value != null)
                    prop.Set(obj, value);
                else
                    propertiesMissed.Add(prop.Name);
            }
            return propertiesMissed;
        }


    }
}