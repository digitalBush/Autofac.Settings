using System.Configuration;
using System;

namespace Autofac.Settings {
    public class AppConfigSettingsProvider : NameValueSettingsProvider {
        public AppConfigSettingsProvider() : base(ConfigurationManager.AppSettings) { }

        public AppConfigSettingsProvider(Func<Type,string,string> keySelector) : base(ConfigurationManager.AppSettings,keySelector) { }
    }
}