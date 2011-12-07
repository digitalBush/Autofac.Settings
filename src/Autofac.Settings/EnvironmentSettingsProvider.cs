using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autofac.Settings {
    public class EnvironmentSettingsProvider : KeyedSettingsProviderBase {

        public EnvironmentSettingsProvider()
            : base((type, prop) => type.Name + "." + prop) { }

        public EnvironmentSettingsProvider(string prefix) :
            base((type, prop) => String.Format("{0}.{1}.{2}", prefix, type.Name, prop)) { }

        public EnvironmentSettingsProvider(Func<Type, string, string> keySelector) :
            base(keySelector) { }

        public override object ProvideValueFor(Type type, string propertyName) {
            return Environment.GetEnvironmentVariable(KeySelector(type, propertyName));
        }
    }
}
