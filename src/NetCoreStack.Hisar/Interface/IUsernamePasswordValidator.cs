namespace NetCoreStack.Hisar
{
    public interface IUsernamePasswordValidator
    {
        bool EnsureHasAnyUsers();
        bool Validate(string username, string password);
    }
}
