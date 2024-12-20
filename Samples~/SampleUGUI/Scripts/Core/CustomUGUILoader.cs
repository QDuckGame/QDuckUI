using QDuck.UI;
using QDuck.UI.UGUI;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Game.Core
{
    public class CustomUGUILoader:UGUILoader
    {
        public override void LoadRes(string uiName, Action<object> callback)
        {
            var go = UnityEngine.Resources.Load<GameObject>(uiName);
            go = Object.Instantiate(go);
            go.name = uiName;
            callback?.Invoke(go);
        }
    }
}