
using UnityEngine;
using QDuck.UI;
namespace Game.Core
{
    public partial class UIMgr
    {
        private static void RuntimeSetting()
        {
            UISetting.DefaultCacheCount = 1;
        }
        
        private static void EditorSetting()
        {
            UISetting.UIViewGenPath = Application.dataPath + "/Samples/QDuckUI/0.0.1/SampleUGUI/Scripts/PanelGen";
            UISetting.UIViewNameSpace = "Game.Panel";
        }

        private static void PanelsSetting(UIContext uiContext)
        {
            Reg("EnterGamePage", new UIPanelInfo() { Id = 10001, Path = "UI/EnterGame/EnterGamePage", SortOrder = 0,BlockRaycast = true});
            Reg("MainPage", new UIPanelInfo() { Id = 10001, Path = "UI/Main/MainPage", SortOrder = 0,BlockRaycast = true});
            Reg("GamePage1", new UIPanelInfo() { Id = 10002, Path = "UI/Page1/GamePage1", SortOrder = 0,BlockRaycast = true});

            Reg("Windows1", new UIPanelInfo() { Id = 20001, Path = "UI/Windows1/Windows1", SortOrder = 10,BlockRaycast = true});
            Reg("Windows2", new UIPanelInfo() { Id = 20002, Path = "UI/Windows2/Windows2", SortOrder = 10,BlockRaycast = true, NeedRetain = true });

            
            void Reg(string name, UIPanelInfo panelInfo)
            {
                uiContext.RegisterPanelInfo(name, panelInfo);
            }
        }
    }
    

}