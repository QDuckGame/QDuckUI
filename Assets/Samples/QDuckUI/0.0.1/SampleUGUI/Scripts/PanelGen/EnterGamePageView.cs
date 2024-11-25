namespace Game.PanelGen
{
using UnityEngine;
using QDuck.UI;

public class EnterGamePageView : UGUIView
{
    public UnityEngine.UI.Button EnterBtn;
    protected override void OnBind()
    {
        EnterBtn = _binder.Components[0] as UnityEngine.UI.Button;

    }
}
}
