namespace Game.PanelGen
{
using UnityEngine;
using QDuck.UI;

public class MainPageView : UGUIView
{
    public UnityEngine.UI.Button OpenGamePage1Btn;
    public UnityEngine.UI.Button OpenWin1Btn;
    protected override void OnBind()
    {
        OpenGamePage1Btn = _binder.Components[0] as UnityEngine.UI.Button;
        OpenWin1Btn = _binder.Components[1] as UnityEngine.UI.Button;

    }
}
}
