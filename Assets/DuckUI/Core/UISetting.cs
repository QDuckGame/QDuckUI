using UnityEngine;

namespace Demo.Core
{
    public class UISetting
    {
        //runtime setting
        public static int DefaultCacheCount = 1;
            
        //editor setting
        public static string UIViewGenPath = Application.dataPath+ "/Assets/Gen";
        public static string UIViewNameSpace = "";
        
    }
}