namespace NetCoreStack.Hisar
{
    public class DefaultUsernamePasswordValidator : IUsernamePasswordValidator
    {
        public bool Validate(string username, string password)
        {
            return true;
        }
    }
}
