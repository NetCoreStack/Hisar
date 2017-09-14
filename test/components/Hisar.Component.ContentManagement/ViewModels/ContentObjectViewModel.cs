using NetCoreStack.Contracts;
using System;

namespace Hisar.Component.ContentManagement
{
    public class ContentObjectViewModel : CollectionModelBson
    {
        public ContentObjectType ContentObjectType { get; set; }

        public string Title { get; set; }

        [PropertyDescriptor(EnableFilter = false)]
        public string Description { get; set; }

        [PropertyDescriptor(EnableFilter = false)]
        public string Url { get; set; }

        [PropertyDescriptor(EnableFilter = false)]
        public DateTime CreatedDate { get; set; }
    }
}
