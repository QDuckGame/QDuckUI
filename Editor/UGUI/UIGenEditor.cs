using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace QDuck.UI.UGUI
{
    public static class UIGenEditor
    {
        
       
        [MenuItem("Assets/DuckUI/GenUIBinder", priority = 0)]
        public static void ShowWindow()
        {
            GameObject[] selectobjs = Selection.gameObjects;

            foreach (GameObject go in selectobjs)
            {
                GenerateClassFromPrefab(go);        
            }
        }

        private static void GenerateClassFromPrefab(GameObject prefab)
        {
            UGUIBehavior behavior = prefab.GetComponent<UGUIBehavior>();
            if (behavior == null)
            {
                behavior =prefab.AddComponent<UGUIBehavior>();
                PrefabUtility.SavePrefabAsset(prefab);
                AssetDatabase.Refresh();
            }
            List<Component> components = new List<Component>();
            
            StringBuilder classBuilder = new StringBuilder();
            StringBuilder initValuesBuilder = new StringBuilder();
            string className = prefab.name;
            
            classBuilder.AppendLine("using UnityEngine;");
            classBuilder.AppendLine("using QDuck.UI;");
            classBuilder.AppendLine();
            
            if (!string.IsNullOrEmpty(UISetting.UIViewNameSpace))
            {
                classBuilder.AppendLine($"namespace {UISetting.UIViewNameSpace}");
                classBuilder.AppendLine("{");
            }

            classBuilder.AppendLine($"    public partial class {className} : IUIComponentBinder");
            classBuilder.AppendLine("    {");
            int index = 0;
            foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
            {
                if (child == prefab.transform) continue;
                var belongPrefab = PrefabUtility.GetOutermostPrefabInstanceRoot(child);
                var element = child.GetComponent<UGUIComponent>();
                bool isParts = false;
                string generateType = "";
                if (belongPrefab != null)
                {
                    if(belongPrefab != child.gameObject)continue;
                    UGUIBehavior childBehavior = belongPrefab.GetComponent<UGUIBehavior>();
                    if (element!=null && childBehavior!=null&& element.generateType == typeof(UGUIBehavior).Name)
                    {
                        GameObject generateClassFromPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(child.gameObject);
                        generateType = generateClassFromPrefab.name;
                        GenerateClassFromPrefab(generateClassFromPrefab);
                        isParts = true;
                    }
                }
                
                if (element == null || string.IsNullOrEmpty(element.generateType)) continue;
                var component = child.GetComponent(element.generateType);
                if(component == null)
                {
                    Debug.LogError($" compoent {element.generateType} not exit");
                    element.generateType = "";
                    continue;
                }
                string typeName = component.GetType().FullName;
                if(isParts) typeName = generateType;
                
                string fieldName = child.name.Replace(" ", "_");
                classBuilder.AppendLine($"        public {typeName} {fieldName};");
                if (isParts)
                {
                    initValuesBuilder.AppendLine($"            {fieldName} = panel.BindBehaviour<{typeName}>(uiBehavior.GetBindComponent({index}) as UGUIBehavior);");
                }
                else
                {
                    initValuesBuilder.AppendLine($"            {fieldName} = uiBehavior.GetBindComponent({index}) as {typeName};");
                }
                components.Add(component);
                index++;
            }
            behavior.Components = components.ToArray();
            classBuilder.AppendLine("        public void BindComponents(IUIBehavior uiBehavior,UIPanel panel)");
            classBuilder.AppendLine("        {");
            classBuilder.AppendLine(initValuesBuilder.ToString());
            classBuilder.AppendLine("        }");
        
            classBuilder.AppendLine("    }");
            if (!string.IsNullOrEmpty(UISetting.UIViewNameSpace))
            {
                classBuilder.AppendLine("}");
            }

            string dirPath = UISetting.UIViewGenPath;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string path = Path.Combine(dirPath, $"{className}Partial.cs");
            File.WriteAllText(path, classBuilder.ToString());
            PrefabUtility.SavePrefabAsset(prefab);
            AssetDatabase.Refresh();

        }
    
    }
}