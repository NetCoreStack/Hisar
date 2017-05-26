namespace NetCoreStack.Hisar
{
    public interface IMenuItem
    {
        string Text { get; set; }
        string Path { get; set; }
        string Icon { get; set; }
        string Parent { get; set; }
        int Order { get; set; }
        bool ShowInMenu { get; set; }
    }
}
