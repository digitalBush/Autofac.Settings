using System.Collections.Specialized;
using Machine.Specifications;

namespace Autofac.Settings.Specs {

    [Subject(typeof(SettingsSource))]
    public class when_resolving_settings {

        class FooSettings {
            public string Bar { get; set; }
        }

        class NeedsSomeSettings {
            public NeedsSomeSettings(FooSettings settings) {
                Settings = settings;
            }

            public FooSettings Settings { get; private set; }
        }

        
        static IContainer _container;
        static NeedsSomeSettings _needsomeSettings;


        Establish context = ()=> {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new SettingsSource());
            builder.RegisterType<SettingsFactory>().AsImplementedInterfaces();
            builder.RegisterType<NameValueSettingsProvider>()
                .WithParameter("source", new NameValueCollection() {{"FooSettings.Bar", "w00t!"}})
                .AsImplementedInterfaces();
            builder.RegisterType<NeedsSomeSettings>().AsSelf();
            _container = builder.Build();
        };

        Because of = () => _needsomeSettings = _container.Resolve<NeedsSomeSettings>();

        It should_resolve_type = () => _needsomeSettings.ShouldNotBeNull();
        It should_have_settings = () => _needsomeSettings.Settings.ShouldNotBeNull();
        It should_have_settings_populated = () => _needsomeSettings.Settings.Bar.ShouldEqual("w00t!");
    }
}