using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Autofac.Settings {
    public class NameValueSettingsProvider : ISettingsProvider {
        private readonly NameValueCollection _source;

        public NameValueSettingsProvider(NameValueCollection source) {
            _source = source;
        }

        public IEnumerable<string> Touch(object obj)
        {
            List<string> propsTouched=new List<string>();
            Type type = obj.GetType();
            var keys=_source.AllKeys.Where(k => k.StartsWith(type.Name));
            foreach (var key in keys)
            {
                var propName = key.Substring(type.Name.Length+1);
                obj.SetProperty(propName,_source[key]);
                propsTouched.Add(propName);
            }
            return propsTouched;
        }
    }
}