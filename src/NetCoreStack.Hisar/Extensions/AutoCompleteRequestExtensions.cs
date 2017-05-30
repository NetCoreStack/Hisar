using NetCoreStack.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public static class AutoCompleteRequestExtensions
    {
        public static bool HasSelectedIds(this AutoCompleteRequest request)
        {
            if (request == null)
            {
                return false;
            }

            if (request.SelectedIds == null)
            {
                return false;
            }

            if (request.SelectedIds.Any())
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<string> GetSelectedIds(this AutoCompleteRequest request)
        {
            if (request.HasSelectedIds())
            {
                return request.SelectedIds;
            }

            return null;
        }
    }
}
