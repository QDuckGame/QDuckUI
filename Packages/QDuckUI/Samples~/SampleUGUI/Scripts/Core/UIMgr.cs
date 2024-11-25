#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using QDuck.UI;

namespace Game.Core
{
    public partial class UIMgr
    {
        private static UIContext _uiContext;

        private void Awake()
        {
            CustomUGUILoader loader = new CustomUGUILoader();
            _uiContext = new UIContext(loader);
            PanelsSetting(_uiContext);
        }

        public static void Open<T>()
            where T : UIPanel, new()
        {
            _uiContext.OpenPanel<T>(typeof(T).Name);
        }

        public static void Close(UIPanel panel)
        {
            _uiContext.ClosePanel(panel);
        }
        
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void OnEditorLoad()
        {
            EditorSetting();
        }
#endif
    }
}