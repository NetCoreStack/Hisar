using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreStack.Hisar.Server
{
    [Area("Admin")]
    public abstract class HisarControllerAdminBase : HisarControllerServerBase
    {
    }
}