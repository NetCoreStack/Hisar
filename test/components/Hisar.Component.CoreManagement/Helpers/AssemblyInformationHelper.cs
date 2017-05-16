using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using NetCoreStack.Hisar;

namespace Hisar.Component.CoreManagement.Helpers
{
    public class AssemblyInformationHelper
    {
        private readonly Assembly _assembly;
        public AssemblyInformationHelper(Assembly assembly)
        {
            _assembly = assembly;
        }

        public T GetAssemblyAttribute<T>() where T : Attribute
        {
            var atribute = _assembly.GetCustomAttribute(typeof(T));
            return (T)Convert.ChangeType(atribute, typeof(T));
        }

        #region Properties
        public string ComponentId => _assembly.GetComponentId();
        public string PackageId => _assembly.GetName()?.Name;
        public string PackageVersion => GetAssemblyAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        public string Authors => "" /*FileVersionInfo.GetVersionInfo(_assembly.Location).CompanyName*/;
        public string Company => GetAssemblyAttribute<AssemblyCompanyAttribute>()?.Company;
        public string Product => GetAssemblyAttribute<AssemblyProductAttribute>()?.Product;
        public string Description => GetAssemblyAttribute<AssemblyDescriptionAttribute>()?.Description;
        public string Copyright => GetAssemblyAttribute<AssemblyCopyrightAttribute>()?.Copyright;
        public string LicenceUrl => "";
        public string ProjectUrl => "";
        public string IconUrl => "";
        public string RepositoryUrl => "";
        public string Tags => "";
        public string ReleaseNotes => "";
        public string NeutrelLanguage => GetAssemblyAttribute<NeutralResourcesLanguageAttribute>()?.CultureName;
        public string Version => "";
        public string FileVersion => GetAssemblyAttribute<AssemblyFileVersionAttribute>()?.Version;


        #endregion

    }
}
