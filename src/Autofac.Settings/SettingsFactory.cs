using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Autofac.Settings {
    public class SettingsFactory : ISettingsFactory {
        readonly IEnumerable<ISettingsProvider> _providers;

        public SettingsFactory(IEnumerable<ISettingsProvider> providers) {
            _providers = providers;
        }

        public object Create(Type type) {
            var obj = Activator.CreateInstance(type);
            var propertiesTouched = new HashSet<string>();
            foreach (var provider in _providers) {
                var touched = provider.Touch(obj);
                propertiesTouched.UnionWith(touched);
            }
            VerifyAllPropertiesGotTouched(type, propertiesTouched);
            return obj;
        }

        static void VerifyAllPropertiesGotTouched(Type type, HashSet<string> properties) {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Select(p => p.Name)
                .ToList();

            var propertiesMissed = props.Except(properties);

            if (propertiesMissed.Any()) {
                var message = String.Format("{0} is missing the following properties in configuration: {1}", type.Name, String.Join(",", propertiesMissed));
                throw new ConfigurationErrorsException(message);
            }
        }
    }
}