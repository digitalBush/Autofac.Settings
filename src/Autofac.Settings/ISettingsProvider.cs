using System.Collections.Generic;
using System;

namespace Autofac.Settings {
    public interface ISettingsProvider {
        object ProvideValueFor(Type type,string propertyName);
    }
}