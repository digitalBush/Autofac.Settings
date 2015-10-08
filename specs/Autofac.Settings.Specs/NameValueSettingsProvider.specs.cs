using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Machine.Specifications;

namespace Autofac.Settings.Specs {

    [Subject(typeof(NameValueSettingsProvider))]
    public class when_providing_value_for_matching_key {
        static NameValueSettingsProvider _provider;
        static string _value;
        static IEnumerable<string> _properties;

        Establish context = () => {
            _provider =
                new NameValueSettingsProvider(new NameValueCollection {
                    {"Settings.Foo", "Yo"}
                });
        };

        Because of = () => _value = _provider.ProvideValueFor(typeof(Settings), "Foo") as string;
        It should_provide_the_correct_value =()=> _value.ShouldEqual("Yo");

        class Settings {
            public string Foo { get; set; }
        }
    }

    [Subject(typeof(NameValueSettingsProvider))]
    public class when_providing_value_for_missing_key {
        static NameValueSettingsProvider _provider;
        static string _value;
        static IEnumerable<string> _properties;

        Establish context = () => {
            _provider =
                new NameValueSettingsProvider(new NameValueCollection {
                    {"Settings.Bar", "Yo"}
                });
        };

        Because of = () => _value = _provider.ProvideValueFor(typeof(Settings), "Foo") as string;
        It should_provide_the_correct_value = () => _value.ShouldBeNull();

        class Settings {
            public string Foo { get; set; }
        }
    }
}