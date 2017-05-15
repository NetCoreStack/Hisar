using Hisar.Component.Guideline.Models;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Mvc.Helpers;
using System;

namespace Hisar.Component.Guideline
{
    public static class BsonDataInitializer
    {
        const string imgUrl = "http://placehold.it/200x100";

        public static void InitializeMusicStoreMongoDb(IServiceProvider serviceProvider)
        {
            if (NetworkHelper.ConnectionCheck(27017))
            {
                using (var db = serviceProvider.GetService<IMongoDbDataContext>())
                {
                    var albums = BsonSampleData.GetAlbums(imgUrl, BsonSampleData.Genres, BsonSampleData.Artists);
                    db.MongoDatabase.DropCollection(db.CollectionNameSelector.GetCollectionName<AlbumBson>());
                    db.Collection<AlbumBson>().InsertMany(albums);
                }
            }
        }
    }
}
