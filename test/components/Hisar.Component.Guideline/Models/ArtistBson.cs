using NetCoreStack.Contracts;
using NetCoreStack.Data.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Guideline.Models
{
    [CollectionName("Artists")]
    public class ArtistBson : EntityIdentityBson
    {
        [Required]
        public string Name { get; set; }
    }
}
