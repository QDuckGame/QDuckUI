using UnityEngine;
using QDuck.UI;

namespace Game.Panel
{
    public partial class Windows1 : IUIComponentBinder
    {
        public UnityEngine.UI.Button OpenWin2Btn;
        public UnityEngine.UI.Button CloseBtn;
        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)
        {
            OpenWin2Btn = uiBehavior.GetBindComponent(0) as UnityEngine.UI.Button;
            CloseBtn = uiBehavior.GetBindComponent(1) as UnityEngine.UI.Button;

        }
    }
}
