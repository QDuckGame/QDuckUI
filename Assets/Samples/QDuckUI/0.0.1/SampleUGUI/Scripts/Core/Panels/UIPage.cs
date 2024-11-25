using QDuck.UI;
namespace Game.Core
{
    public class UIPage<T>:UIPanel<T>
    where T: IUIView, new()
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