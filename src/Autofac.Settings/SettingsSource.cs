using System;
using System.Collections.Generic;
using Autofac.Builder;
using Autofac.Core;


namespace Autofac.Settings {
    using Builder = IRegistrationBuilder<object,SimpleActivatorData,SingleRegistrationStyle>;
    public class SettingsSource : IRegistrationSource {
        readonly Func<Builder, Builder> _registration;

        public SettingsSource():this(x=>x.SingleInstance()){}

        public SettingsSource(Func<Builder,Builder> registration)
        {
            _registration = registration;
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor) {
            var s = service as IServiceWithType;
            if (s != null && s.ServiceType.IsClass && s.ServiceType.Name.EndsWith("Settings"))
            {
                var builder = RegistrationBuilder.ForDelegate((c, p) => c.Resolve<ISettingsFactory>().Create(s.ServiceType))
                    .As(s.ServiceType);

                builder = _registration(builder);
                yield return builder.CreateRegistration();
            }
        }

        public bool IsAdapterForIndividualComponents {
            get { return false; }
        }
    }
}
