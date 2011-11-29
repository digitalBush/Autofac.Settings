using Machine.Specifications;
using System.Reflection;

namespace Autofac.Settings.Specs {

    [Subject(typeof(PropertyHelper))]
    public class when_setting_a_property_with_helper {
        static Settings _settings;
        
        Establish context = () => _settings = new Settings();
        Because of = () => typeof(Settings).GetProperty("Test").Set(_settings, "Foo");
        It should_have_the_correct_value = () => _settings.Test.ShouldEqual("Foo");

        class Settings {
            public string Test { get; set; }
        }
    }
}