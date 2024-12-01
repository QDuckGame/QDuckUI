using QDuck.UI;
namespace Game.Core
{
    public class UIWindow<T>:UGUIPanel<T>
    where T: UGUIView, new()
    {
        private static UIPanelStackInfo UIWindowConfig = new UIPanelStackInfo()
        {
            Priority = 5,
            Compare = true,
            StackType = PanelStackType.Push,
        };
        
        public override UIPanelStackInfo StackInfo => UIWindowConfig;
    }
}