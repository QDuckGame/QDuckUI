using UnityEngine;
using QDuck.UI;

namespace Game.Panel
{
    public partial class Windows2 : IUIComponentBinder
    {
        public UnityEngine.UI.Button CloseBtn;
        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)
        {
            CloseBtn = uiBehavior.GetBindComponent(0) as UnityEngine.UI.Button;

        }
    }
}
