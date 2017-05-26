namespace NetCoreStack.Hisar
{
    public class DefaultMenuItem : IMenuItem
    {
        public string Text { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public string Parent { get; set; }
        public int Order { get; set; }
        public bool ShowInMenu { get; set; }
    }
}
