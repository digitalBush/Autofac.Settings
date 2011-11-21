using System.Collections.Specialized;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Settings.Specs {

    class FooSettings {
        public string Bar { get; set; }
    }

    class NeedsSomeSettings {
        public NeedsSomeSettings(FooSettings settings){
            Settings = settings;
        }

        public FooSettings Settings { get; private set; }
    }

    public class when_resolving_a_class_that_depends_on_settings {    
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

    public class when_resolving_settings_multiple_times {

        static IContainer _container;
        static IList<FooSettings> _settings;

        Establish context = () => {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new SettingsSource());
            builder.RegisterType<SettingsFactory>().AsImplementedInterfaces();
            builder.RegisterType<NameValueSettingsProvider>()
                .WithParameter("source", new NameValueCollection() { { "FooSettings.Bar", "w00t!" } })
                .AsImplementedInterfaces();
            _container = builder.Build();
        };

        Because of = () => _settings = Enumerable.Range(0, 2).Select(i => _container.Resolve<FooSettings>()).ToList();

        It should_resolve_settings = () => _settings[0].ShouldNotBeNull();
        It should_return_the_same_instance = () => _settings[0].ShouldBeTheSameAs(_settings[1]);

    }
}