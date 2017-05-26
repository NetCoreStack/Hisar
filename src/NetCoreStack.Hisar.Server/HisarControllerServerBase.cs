using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetCoreStack.Contracts;
using NetCoreStack.Data;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Data.Types;

namespace NetCoreStack.Hisar.Server
{
    public abstract class HisarControllerServerBase : HisarControllerBase
    {
        private IMongoUnitOfWork _unitOfWork;
        public IMongoUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                    _unitOfWork = Resolver?.GetService<IMongoUnitOfWork>();

                return _unitOfWork;
            }
        }
    }
}