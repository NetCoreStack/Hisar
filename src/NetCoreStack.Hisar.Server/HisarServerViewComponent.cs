using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;

namespace NetCoreStack.Hisar.Server
{
    public class HisarServerViewComponent : HisarViewComponent
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