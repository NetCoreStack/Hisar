using NetCoreStack.Hisar;

namespace Hisar.Component.Guideline
{
    public class GuidelineUsernamePasswordValidator : IUsernamePasswordValidator
    {
        public bool EnsureHasAnyUsers()
        {
            return true;
        }

        public bool Validate(string username, string password)
        {
            return true;
        }
    }
}
