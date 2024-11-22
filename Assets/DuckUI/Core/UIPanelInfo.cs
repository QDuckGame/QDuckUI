namespace Duck.UI
{
    public enum PanelStackType
    {
        DontPush,
        SinglePush,
        Push,
    }

    public struct UIPanelStackInfo
    {
        public int Priority;
        public bool Compare;
        public PanelStackType StackType;

    }

    public struct UIPanelInfo
    {
        public int Id;
        public string Path;
        public int SortOrder;
        public bool NeedRetain;
    }
}
