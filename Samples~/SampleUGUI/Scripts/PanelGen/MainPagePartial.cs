using UnityEngine;
using QDuck.UI;

namespace Game.Panel
{
    public partial class MainPage : IUIComponentBinder
    {
        public UnityEngine.UI.Button OpenGamePage1Btn;
        public UnityEngine.UI.Button OpenWin1Btn;
        public ContenParts ContenParts;
        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)
        {
            OpenGamePage1Btn = uiBehavior.GetBindComponent(0) as UnityEngine.UI.Button;
            OpenWin1Btn = uiBehavior.GetBindComponent(1) as UnityEngine.UI.Button;
            ContenParts = panel.BindBehaviour<ContenParts>(uiBehavior.GetBindComponent(2) as UGUIBehavior);

        }
    }
}
