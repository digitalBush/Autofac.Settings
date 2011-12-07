using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autofac.Settings {
    public abstract class KeyedSettingsProviderBase : ISettingsProvider {

        protected Func<Type, string, string> KeySelector { get; private set; }

        public KeyedSettingsProviderBase(Func<Type, string, string> keySelector) {
            KeySelector = keySelector;
        }

        public abstract object ProvideValueFor(Type type, string propertyName);
    }
}
