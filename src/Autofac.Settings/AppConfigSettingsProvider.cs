using System.Configuration;

namespace Autofac.Settings {
    public class AppConfigSettingsProvider : NameValueSettingsProvider {
        public AppConfigSettingsProvider() : base(ConfigurationManager.AppSettings) { }
    }
}