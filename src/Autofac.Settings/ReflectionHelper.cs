using System;
using System.ComponentModel;
using System.Configuration;

namespace Autofac.Settings {
    public static class ReflectionHelper
    {
        public static void SetProperty(this object obj,string propertyName,object value)
        {
            var type = obj.GetType();
            var prop=type.GetProperty(propertyName);
            if(prop==null)
                throw new ConfigurationErrorsException(String.Format("\"{0}\" is not a valid property for {1}",propertyName,type.Name));
            var converter = TypeDescriptor.GetConverter(prop.PropertyType);
            var newValue = converter.ConvertFrom(value);
            prop.SetValue(obj, newValue, null);
        }
    }
}