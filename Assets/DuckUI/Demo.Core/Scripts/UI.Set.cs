
using UnityEngine;
using Duck.UI;
namespace Demo.Core
{
    public partial class UI
    {
        private static void RuntimeSetting()
        {
            UISetting.DefaultCacheCount = 1;
        }
        
        private static void EditorSetting()
        {
            UISetting.UIViewGenPath = Application.dataPath + "/DuckUI/Demo.UI/UIGen";
            UISetting.UIViewNameSpace = "Demo";
        }

        private static void PanelsSetting(UIContext uiContext)
        {
            Reg("UIMainPage", new UIPanelInfo() { Id = 10001, Path = "UIMainPage", SortOrder = 0 });
            Reg("UIShopPage", new UIPanelInfo() { Id = 10002, Path = "UIShopPage", SortOrder = 0 });

            Reg("UIAWindow", new UIPanelInfo() { Id = 20001, Path = "UIAWindow", SortOrder = 10 });
            Reg("UIBWindow", new UIPanelInfo() { Id = 20002, Path = "UIBWindow", SortOrder = 10, NeedRetain = true });
            Reg("UICWindow", new UIPanelInfo() { Id = 20003, Path = "UICWindow", SortOrder = 10 });

            
            void Reg(string name, UIPanelInfo panelInfo)
            {
                uiContext.RegisterPanelInfo(name, panelInfo);
            }
        }
    }
    

}