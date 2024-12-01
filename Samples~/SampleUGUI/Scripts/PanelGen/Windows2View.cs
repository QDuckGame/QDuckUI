namespace Game.PanelGen
{
using UnityEngine;
using QDuck.UI;

public class Windows2View : UGUIView
{
    public UnityEngine.UI.Button CloseBtn;
    protected override void OnBind()
    {
        CloseBtn = _binder.Components[0] as UnityEngine.UI.Button;

    }
}
}
