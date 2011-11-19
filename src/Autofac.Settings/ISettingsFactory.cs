using System;

namespace Autofac.Settings {
    public interface ISettingsFactory {
        object Create(Type type);
    }
}