namespace Game.PanelGen
{
using UnityEngine;
using QDuck.UI;

public class Windows1View : UGUIView
{
    public UnityEngine.UI.Button OpenWin2Btn;
    public UnityEngine.UI.Button CloseBtn;
    protected override void OnBind()
    {
        OpenWin2Btn = _binder.Components[0] as UnityEngine.UI.Button;
        CloseBtn = _binder.Components[1] as UnityEngine.UI.Button;

    }
}
}
