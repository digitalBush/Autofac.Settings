using Machine.Specifications;

namespace Autofac.Settings.Specs {

    [Subject(typeof(ReflectionHelper))]
    public class when_setting_a_property_with_helper {
        static Settings _settings;

        Establish context = () => _settings = new Settings();
        Because of = () => _settings.SetProperty("Test", "Foo");
        It should_have_the_correct_value = () => _settings.Test.ShouldEqual("Foo");

        class Settings {
            public string Test { get; set; }
        }
    }
}