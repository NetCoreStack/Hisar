namespace NetCoreStack.Hisar
{
    public interface IUsernamePasswordValidator
    {
        bool Validate(string username, string password);
    }
}
