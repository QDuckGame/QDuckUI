using UnityEngine;
using QDuck.UI;

namespace Game.Panel
{
    public partial class ContenParts : IUIComponentBinder
    {
        public UnityEngine.UI.Button TestBtn;
        public TMPro.TextMeshProUGUI Content;
        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)
        {
            TestBtn = uiBehavior.GetBindComponent(0) as UnityEngine.UI.Button;
            Content = uiBehavior.GetBindComponent(1) as TMPro.TextMeshProUGUI;

        }
    }
}
