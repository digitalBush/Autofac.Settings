using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Autofac.Settings {
    public class NameValueSettingsProvider : ISettingsProvider {
        private readonly NameValueCollection _source;

        public NameValueSettingsProvider(NameValueCollection source) {
            _source = source;
        }

        public object ProvideValueFor(Type type, string propertyName) {
            var key = type.Name + "." + propertyName;
            return _source[key];
        }
    }
}