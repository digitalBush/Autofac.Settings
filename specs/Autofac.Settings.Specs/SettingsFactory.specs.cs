using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Machine.Specifications;
using Moq;
using Arg=Moq.It;
using It = Machine.Specifications.It;


namespace Autofac.Settings.Specs {

    [Subject(typeof(SettingsFactory))]
    public class when_creating_settings_where_all_properties_specified {
        static Mock<ISettingsProvider> _provider;
        static SettingsFactory _factory;
        static Settings _settings;

        Establish context = () => {
            _provider = new Mock<ISettingsProvider>();
            _provider.Setup(x => x.ProvideValueFor(typeof(Settings),"Foo")).Returns("Test").Verifiable();
            _provider.Setup(x => x.ProvideValueFor(typeof(Settings), "Bar")).Returns("1").Verifiable();
            _provider.Setup(x => x.ProvideValueFor(typeof(Settings), "Baz")).Returns("3.14").Verifiable();
            _factory=new SettingsFactory(new[]{_provider.Object});
        };

        Because of = () => _settings = _factory.Create(typeof (Settings)) as Settings;

        It should_create_settings_object = () => _settings.ShouldNotBeNull();
        It should_call_settings_provider = () => _provider.Verify();
        It should_have_the_correct_string_property = () => _settings.Foo.ShouldEqual("Test");
        It should_have_the_correct_int_property = () => _settings.Bar.ShouldEqual(1);
        It should_have_the_correct_decimal_property = () => _settings.Baz.ShouldEqual(3.14m);

        class Settings {
            public string Foo { get; set; }
            public int Bar { get; set; }
            public decimal Baz { get; set; }
        }

    }

    [Subject(typeof(SettingsFactory))]
    public class when_creating_settings_where_NOT_all_properties_specified {
        static Mock<ISettingsProvider> _provider;
        static SettingsFactory _factory;
        static Exception _exception;

        Establish context = () => {
            _provider = new Mock<ISettingsProvider>();
            _provider.Setup(x => x.ProvideValueFor(typeof(Settings), "Foo")).Returns("Test").Verifiable();
            _factory = new SettingsFactory(new[] { _provider.Object });
        };

        Because of = () => _exception=Catch.Exception(()=> _factory.Create(typeof(Settings)));

        It should_throw_configuration_errors_exception = () => _exception.ShouldBeOfExactType<ConfigurationErrorsException>();
        It should_have_exception_message_with_missed_properties = () => _exception.Message.ShouldEndWith("Bar,Baz");
        It should_call_settings_provider = () => _provider.Verify();

        class Settings {
            public string Foo { get; set; }
            public int Bar { get; set; }
            public decimal Baz { get; set; }
        }

    }

    [Subject(typeof(SettingsFactory))]
    public class when_creating_settings_from_multiple_providers {
        static List<Mock<ISettingsProvider>> _providers;
        static SettingsFactory _factory;
        static Settings _settings;

        Establish context = () => {
            var p1 = new Mock<ISettingsProvider>();
            p1.Setup(x => x.ProvideValueFor(typeof(Settings), "Foo")).Returns("Test").Verifiable();

            var p2 = new Mock<ISettingsProvider>();
            p2.Setup(x => x.ProvideValueFor(typeof(Settings), "Bar")).Returns("1").Verifiable();

            _providers=new List<Mock<ISettingsProvider>>(){p1,p2};

            _factory = new SettingsFactory(_providers.Select(x=>x.Object));
        };

        Because of = () => _settings = _factory.Create(typeof(Settings)) as Settings;

        It should_create_settings_object = () => _settings.ShouldNotBeNull();
        It should_call_all_settings_provider = () => _providers.ForEach(x => x.Verify());
        It should_have_populated_first_property = () => _settings.Foo.ShouldEqual("Test");
        It should_have_populated_other_property = () => _settings.Bar.ShouldEqual(1);

        class Settings {
            public string Foo { get; set; }
            public int Bar { get; set; }
        }

    }
}