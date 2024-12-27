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
    
}
