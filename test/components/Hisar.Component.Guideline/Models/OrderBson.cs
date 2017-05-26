using NetCoreStack.Contracts;
using System;
using System.Collections.Generic;

namespace Hisar.Component.Guideline.Models
{
    [CollectionName("Orders")]
    public class OrderBson
    {
        public DateTime PurchaseDate { get; set; }

        public IList<AlbumBson> Items { get; set; }
    }
}
