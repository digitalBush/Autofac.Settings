using System.Collections.Generic;

namespace Autofac.Settings {
    public interface ISettingsProvider {
        IEnumerable<string> Touch(object obj);
    }
}