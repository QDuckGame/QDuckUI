using Duck.UI;
using Duck.UI.UGUI;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Demo.Core
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