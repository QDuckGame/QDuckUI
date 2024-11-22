using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using Demo.Core;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Duck.UI.UGUI
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
            UGUIBinder binder = prefab.GetComponent<UGUIBinder>();
            if (binder == null)
            {
                binder =prefab.AddComponent<UGUIBinder>();
                PrefabUtility.SavePrefabAsset(prefab);
                AssetDatabase.Refresh();
            }
            List<Component> components = new List<Component>();
            
            StringBuilder classBuilder = new StringBuilder();
            StringBuilder initValuesBuilder = new StringBuilder();
            string className = prefab.name;
            if (!string.IsNullOrEmpty(UISetting.UIViewNameSpace))
            {
                classBuilder.AppendLine($"namespace {UISetting.UIViewNameSpace}");
                classBuilder.AppendLine("{");
            }
            classBuilder.AppendLine("using UnityEngine;");
            classBuilder.AppendLine();
            classBuilder.AppendLine($"public class {className}View : UGUIView");
            classBuilder.AppendLine("{");
            int index = 0;
            foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
            {
                if (child == prefab.transform) continue;
                var belongPrefab = PrefabUtility.GetOutermostPrefabInstanceRoot(child);
                if (belongPrefab != null)
                {
                    if(belongPrefab != child.gameObject)continue;
                    GenerateClassFromPrefab(PrefabUtility.GetCorrespondingObjectFromOriginalSource(child.gameObject));
                }
                var element = child.GetComponent<UGUIElement>();
                if (element == null || string.IsNullOrEmpty(element.generateType)) continue;
                var component = child.GetComponent(element.generateType);
                if(component == null)
                {
                    Debug.LogError($" compoent {element.generateType} not exit");
                    element.generateType = "";
                    continue;
                }
                string typeName = component.GetType().FullName;
                string fieldName = child.name.Replace(" ", "_");
                classBuilder.AppendLine($"    public {typeName} {fieldName};");
                initValuesBuilder.AppendLine($"        {fieldName} = _binder.Components[{index}] as {typeName};");
                components.Add(component);
                index++;
            }
            binder.Components = components.ToArray();
            classBuilder.AppendLine("    protected override void OnBind()");
            classBuilder.AppendLine("    {");
            classBuilder.AppendLine(initValuesBuilder.ToString());
            classBuilder.AppendLine("    }");
        
            classBuilder.AppendLine("}");
            if (!string.IsNullOrEmpty(UISetting.UIViewNameSpace))
            {
                classBuilder.AppendLine("}");
            }

            string dirPath = UISetting.UIViewGenPath;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string path = Path.Combine(dirPath, $"{className}View.cs");
            File.WriteAllText(path, classBuilder.ToString());
            PrefabUtility.SavePrefabAsset(prefab);
            AssetDatabase.Refresh();

        }
    
    }
}