using NetCoreStack.Contracts;
using NetCoreStack.Data.Contracts;
using NetCoreStack.Hisar.Server;
using System;
using Xunit;

namespace NetCoreStack.Hisar.Tests
{
    [CollectionName("WithAttribute")]
    public class BsonCollectionWithAttribute
    {
    }

    public class GuidelineEntity : EntityIdentityBson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class GuidelineEntityActive : EntityIdentityActiveBson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class CollectionNameTests
    {
        [Fact]
        public void Resolve_CollectionNames()
        {
            HisarCollectionNameSelector selector = new HisarCollectionNameSelector();
            var collectionName = selector.GetCollectionName<GuidelineEntityActive>();
            Assert.True(collectionName == "Tests." + nameof(GuidelineEntityActive));
        }
    }
}
