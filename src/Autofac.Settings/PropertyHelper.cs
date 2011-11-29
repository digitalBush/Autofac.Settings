using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Autofac.Settings {
    public static class PropertyHelper {
        public static void Set(this PropertyInfo prop, object obj, object value) {
            var type = obj.GetType();
            var converter = TypeDescriptor.GetConverter(prop.PropertyType);
            var newValue = converter.ConvertFrom(value);
            prop.SetValue(obj, newValue, null);
        }
    }
}
