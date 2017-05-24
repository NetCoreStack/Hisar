namespace NetCoreStack.Hisar
{
    public class DefaultUsernamePasswordValidator : IUsernamePasswordValidator
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
