using Microsoft.AspNetCore.DataProtection;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Extensions;

namespace NetCoreStack.Hisar
{
    public class DefaultDataProtectorProvider : IDataProtectorProvider
    {
        private readonly IDataProtector _protector;
        public DefaultDataProtectorProvider(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("hisar");
        }

        public string Protect(string input)
        {
            string protectedPayload = _protector.Protect(input);
            return protectedPayload;
        }

        public string Unprotect(string protectedPayload)
        {
            string unprotectedPayload = _protector.Unprotect(protectedPayload);
            return unprotectedPayload;
        }

        public string ToBase64(string input)
        {
            return input.ToBase64();
        }

        public string FromBase64(string payload)
        {
            return payload.FromBase64();
        }
    }
}