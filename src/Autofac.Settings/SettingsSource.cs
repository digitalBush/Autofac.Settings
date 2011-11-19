using System;
using System.Collections.Generic;
using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Settings {
    public class SettingsSource : IRegistrationSource {
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor) {
            var s = service as IServiceWithType;
            if (s != null && s.ServiceType.IsClass && s.ServiceType.Name.EndsWith("Settings")) {
                yield return RegistrationBuilder.ForDelegate((c, p) => c.Resolve<ISettingsFactory>().Create(s.ServiceType))
                    .As(s.ServiceType)
                    .SingleInstance()
                    .CreateRegistration();
            }
        }

        public bool IsAdapterForIndividualComponents {
            get { return false; }
        }
    }
}
