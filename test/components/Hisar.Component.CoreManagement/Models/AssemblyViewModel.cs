using System.Collections.Generic;

namespace Hisar.Component.CoreManagement.Models
{
    public class AssemblyViewModel
    {
        public AssemblyViewModel()
        {
            Components = new List<ComponentViewModel>();
        }
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string Authors { get; set; }
        public string Company { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string LicenceUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string IconUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public string Tags { get; set; }
        public string ReleaseNotes { get; set; }
        public string NeutrelLanguage { get; set; }
        public string Version { get; set; }
        public string FileVersion { get; set; }
        public List<ComponentViewModel> Components { get; set; }
    }
}
