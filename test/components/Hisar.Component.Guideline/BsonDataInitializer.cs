using Hisar.Component.Guideline.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Mvc.Helpers;
using System;

namespace Hisar.Component.Guideline
{
    public static class BsonDataInitializer
    {
        const string imgUrl = "http://placehold.it/200x100";

        public static bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return collections.Any();
        }

        public static void InitializeMusicStoreMongoDb(IServiceProvider serviceProvider)
        {
            if (NetworkHelper.ConnectionCheck(27017))
            {
                using (var db = serviceProvider.GetService<IMongoDbDataContext>())
                {
                    if (!CollectionExists(db.MongoDatabase, db.CollectionNameSelector.GetCollectionName<AlbumBson>()))
                    {
                        // db.MongoDatabase.DropCollection(db.CollectionNameSelector.GetCollectionName<AlbumBson>());
                        var albums = BsonSampleData.GetAlbums(imgUrl, BsonSampleData.Genres, BsonSampleData.Artists);
                        db.Collection<AlbumBson>().InsertMany(albums);
                    }
                }
            }
        }
    }
}
