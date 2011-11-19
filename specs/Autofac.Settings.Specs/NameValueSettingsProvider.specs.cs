using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Machine.Specifications;

namespace Autofac.Settings.Specs {
    [Subject(typeof (NameValueSettingsProvider))]
    public class when_touching_object_with_primitive_properties {
        static Settings _instance;
        static NameValueSettingsProvider _provider;
        static IEnumerable<string> _properties;

        Establish context = () => {
            _instance = new Settings();
            _provider =
                new NameValueSettingsProvider(new NameValueCollection {
                    {"Settings.Foo", "Yo"},
                    {"Settings.Bar", "42"},
                    {"Settings.Baz", "3.14"}
                });
        };

        Because of = () => _properties = _provider.Touch(_instance);

        It should_have_the_right_value_for_Bar_property = () => _instance.Bar.ShouldEqual(42);
        It should_have_the_right_value_for_Baz_property = () => _instance.Baz.ShouldEqual(3.14m);
        It should_have_the_right_value_for_Foo_property = () => _instance.Foo.ShouldEqual("Yo");
        It should_have_touched_all_properties = () => _properties.ShouldContainOnly("Foo", "Bar", "Baz");

        #region Nested type: Settings

        class Settings {
            public string Foo { get; set; }
            public int Bar { get; set; }
            public decimal Baz { get; set; }
        }

        #endregion
    }

    [Subject(typeof (NameValueSettingsProvider))]
    public class when_setting_a_property_with_an_incorrect_name {
        static Settings _instance;
        static NameValueSettingsProvider _provider;
        static Exception _exception;

        Establish context = () => {
            _instance = new Settings();
            _provider =
                new NameValueSettingsProvider(new NameValueCollection {{"Settings.No", "Bad"}});
        };

        Because of = () => _exception = Catch.Exception(() => _provider.Touch(_instance));

        It should_report_the_invalid_configuration_item = () => _exception.Message.ShouldStartWith("\"No\"");
        It should_throw_exception = () => _exception.ShouldBeOfType<ConfigurationErrorsException>();

        #region Nested type: Settings

        class Settings {
            public string Foo { get; set; }
        }

        #endregion
    }
}