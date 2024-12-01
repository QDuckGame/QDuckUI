namespace Game.PanelGen
{
using UnityEngine;
using QDuck.UI;

public class GamePage1View : UGUIView
{
    public UnityEngine.UI.Button CloseBtnBtn;
    public UnityEngine.UI.Button OpenWin2Btn;
    protected override void OnBind()
    {
        CloseBtnBtn = _binder.Components[0] as UnityEngine.UI.Button;
        OpenWin2Btn = _binder.Components[1] as UnityEngine.UI.Button;

    }
}
}
