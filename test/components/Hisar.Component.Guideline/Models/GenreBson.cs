using NetCoreStack.Contracts;
using NetCoreStack.Data.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Guideline.Models
{
    [CollectionName("Genres")]
    public class GenreBson : EntityIdentityBson
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
