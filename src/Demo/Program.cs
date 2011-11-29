using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Settings;

namespace Demo {


    class Program {
        static void Main(string[] args) {
            var builder = new ContainerBuilder();
            builder.RegisterType<Worker>();
            builder.RegisterSource(new SettingsSource());
            builder.RegisterType<SettingsFactory>().AsImplementedInterfaces();
            builder.RegisterType<AppConfigSettingsProvider>().AsImplementedInterfaces();
            var container = builder.Build();
            container.Resolve<Worker>().DoWork();
            Console.ReadKey();
        }
    }
}
