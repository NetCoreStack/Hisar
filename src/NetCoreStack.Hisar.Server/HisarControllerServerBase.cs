using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;

namespace NetCoreStack.Hisar.Server
{
    public abstract class HisarControllerServerBase : HisarControllerBase
    {
        private IBsonUnitOfWork _bsonUnitOfWork;
        public IBsonUnitOfWork BsonUnitOfWork
        {
            get
            {
                if (_bsonUnitOfWork == null)
                    _bsonUnitOfWork = Resolver?.GetService<IBsonUnitOfWork>();

                return _bsonUnitOfWork;
            }
        }
    }
}