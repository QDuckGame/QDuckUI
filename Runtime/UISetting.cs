using UnityEngine;

namespace QDuck.UI
{
    public partial class UISetting
    {
        //runtime setting
        public static int DefaultCacheCount = 1;
            
        //editor setting
        public static string UIViewGenPath = Application.dataPath + "/Samples/QDuckUI/0.0.1/SampleUGUI/Scripts/PanelGen";
        public static string UIViewNameSpace = "Game.Panel";
        
    }
}