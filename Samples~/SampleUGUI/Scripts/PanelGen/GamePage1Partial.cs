using UnityEngine;
using QDuck.UI;

namespace Game.Panel
{
    public partial class GamePage1 : IUIComponentBinder
    {
        public UnityEngine.UI.Button CloseBtnBtn;
        public UnityEngine.UI.Button OpenWin2Btn;
        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)
        {
            CloseBtnBtn = uiBehavior.GetBindComponent(0) as UnityEngine.UI.Button;
            OpenWin2Btn = uiBehavior.GetBindComponent(1) as UnityEngine.UI.Button;

        }
    }
}
