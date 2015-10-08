using Autofac;
using Autofac.Settings;
using System;

namespace DemoModule
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacSettingsModule());

            builder.RegisterType<Worker>();
            var container = builder.Build();
            container.Resolve<Worker>().DoWork();


            Console.ReadKey();
        }
    }
}
 