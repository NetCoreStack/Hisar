using NetCoreStack.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Guideline.Models
{
    public class AlbumViewModel : CollectionModelBson
    {
        [PropertyDescriptor(EnableFilter = true, IsSelectable = true)]
        [Required]
        public long GenreId { get; set; }

        public string Genre { get; set; }

        [PropertyDescriptor(EnableFilter = true, IsSelectable = true, DataSourceUrl = "/home/getartistlist")]
        [Required]
        public long ArtistId { get; set; }

        [PropertyDescriptor(EnableFilter = true, DefaultFilterBehavior = FilterOperator.Contains)]
        public string Artist { get; set; }

        [PropertyDescriptor(EnableFilter = true)]
        [Required]
        public string Title { get; set; }

        [PropertyDescriptor(EnableFilter = true)]
        [Required]
        public decimal Price { get; set; }

        [PropertyDescriptor(EnableFilter = true)]
        public string AlbumArtUrl { get; set; }

        public string ProtectedUrl { get; set; }

        [PropertyDescriptor(EnableFilter = false)]
        public DateTime Date { get; set; }

        [PropertyDescriptor(EnableFilter = true)]
        public DateTime? UpdatedDate { get; set; }
    }
}
