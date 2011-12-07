using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Autofac.Settings {
    public class NameValueSettingsProvider : KeyedSettingsProviderBase {
        private readonly NameValueCollection _source;

        public NameValueSettingsProvider(NameValueCollection source): 
            base((type, prop) => type.Name + "." + prop) {
            _source = source;
        }

        public NameValueSettingsProvider(NameValueCollection source, Func<Type,string,string> keySelector) :
            base(keySelector) {
            _source = source;
        }

        public override object ProvideValueFor(Type type, string propertyName) {
            return _source[KeySelector(type,propertyName)];
        }
    }
}