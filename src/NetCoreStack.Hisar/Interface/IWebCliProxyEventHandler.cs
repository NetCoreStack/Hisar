using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public interface IWebCliProxyEventHandler
    {
        Task FileChanged(FileChangedContext context);
    }
}
