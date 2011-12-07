using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Autofac.Settings {
    public abstract class KeyedSettingsProviderBase:ISettingsProvider{

        protected Func<Type, string, string> KeySelector { get; private set; }
        
        public KeyedSettingsProviderBase(Func<Type, string, string> keySelector) {
            KeySelector = keySelector;
        }

        public abstract object ProvideValueFor(Type type, string propertyName);
    }

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

    public class EnvironmentSettingsProvider : KeyedSettingsProviderBase {

        public EnvironmentSettingsProvider()
            : base((type, prop) => type.Name + "." + prop) {}

        public EnvironmentSettingsProvider(string prefix): 
            base((type, prop) => String.Format("{0}.{1}.{2}",prefix,type.Name,prop)) {}

        public EnvironmentSettingsProvider(Func<Type,string,string> keySelector) :
            base(keySelector) { }

        public override object ProvideValueFor(Type type, string propertyName) {
            return Environment.GetEnvironmentVariable(KeySelector(type, propertyName));
        }
    }
}