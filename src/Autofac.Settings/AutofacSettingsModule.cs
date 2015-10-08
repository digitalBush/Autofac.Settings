using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autofac.Settings
{
    public class AutofacSettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new SettingsSource());
            builder.RegisterType<SettingsFactory>().AsImplementedInterfaces();
            builder.RegisterType<AppConfigSettingsProvider>().AsImplementedInterfaces();
        }
    }
}
