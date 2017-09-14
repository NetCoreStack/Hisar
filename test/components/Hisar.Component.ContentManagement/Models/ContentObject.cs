using NetCoreStack.Hisar;
using System;

namespace Hisar.Component.ContentManagement
{
    public class ContentObject : EntityIdentityActiveBson
    {
        public ContentObjectType ContentObjectType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
