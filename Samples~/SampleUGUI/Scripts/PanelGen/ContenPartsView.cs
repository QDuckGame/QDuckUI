namespace Game.PanelGen
{
using UnityEngine;
using QDuck.UI;

public class ContenPartsView : UGUIView
{
    public UnityEngine.UI.Button TestBtn;
    public TMPro.TextMeshProUGUI Content;
    protected override void OnBind()
    {
        TestBtn = _binder.Components[0] as UnityEngine.UI.Button;
        Content = _binder.Components[1] as TMPro.TextMeshProUGUI;

    }
}
}
