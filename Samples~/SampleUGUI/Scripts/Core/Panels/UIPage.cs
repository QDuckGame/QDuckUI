using QDuck.UI;
namespace Game.Core
{
    public class UIPage:UGUIPanel
    {
        private static UIPanelStackInfo UIPageStackConfig = new UIPanelStackInfo()
        {
            Priority = 10,
            Compare = true,
            StackType = PanelStackType.Push,
        };
        
        public override UIPanelStackInfo StackInfo => UIPageStackConfig;
        
    }
}