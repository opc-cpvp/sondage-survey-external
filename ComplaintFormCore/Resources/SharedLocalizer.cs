using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ComplaintFormCore.Resources
{
    public class SharedLocalizer
    {
        private readonly IStringLocalizer _localizerSubmission;
        private readonly IStringLocalizer _localizerGeneral;
        private readonly IStringLocalizer _localizerResource;

        public SharedLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizerSubmission = factory.Create("Submission", assemblyName.Name);
            _localizerGeneral = factory.Create("General", assemblyName.Name);
            _localizerResource = factory.Create("Resource", assemblyName.Name);
        }

        public LocalizedString this[string key] => _localizerSubmission[key];

        public LocalizedString GetLocalizedStringSubmission(string key)
        {
            return _localizerSubmission[key];
        }

        public LocalizedString GetLocalizedStringGeneral(string key)
        {
            return _localizerGeneral[key];
        }

        public LocalizedString GetLocalizedStringResource(string key)
        {
            return _localizerResource[key];
        }
    }
}
