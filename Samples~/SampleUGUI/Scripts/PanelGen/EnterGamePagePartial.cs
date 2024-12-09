using UnityEngine;
using QDuck.UI;

namespace Game.Panel
{
    public partial class EnterGamePage : IUIComponentBinder
    {
        public UnityEngine.UI.Button EnterBtn;
        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)
        {
            EnterBtn = uiBehavior.GetBindComponent(0) as UnityEngine.UI.Button;

        }
    }
}
