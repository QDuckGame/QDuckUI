namespace QDuck.UI
{
    public enum PanelStackType
    {
        DontPush,
        SinglePush,
        Push,
    }

    public partial class UIPanelStackInfo
    {
        public int Priority;
        public bool Compare;
        public PanelStackType StackType;

    }

    public partial class UIPanelInfo
    {
        public int Id;
        public string Path;
        public int SortOrder;
        public bool NeedRetain;
        public bool BlockRaycast;
    }
}
