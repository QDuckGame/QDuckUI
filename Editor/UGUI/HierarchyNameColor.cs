using UnityEngine;
using UnityEditor;
namespace QDuck.UI.UGUI
{
    [InitializeOnLoad]
    public static class HierarchyNameColor
    {
        static HierarchyNameColor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }
        private static GUIStyle _style = new GUIStyle() { alignment = TextAnchor.LowerRight ,fontSize = 6};
        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj != null)
            {
                var elementType = obj.GetComponent<UGUIComponent>();
            
                if (elementType!= null)
                {
                    if (string.IsNullOrEmpty(elementType.generateType))
                    {
                        _style.normal.textColor = Color.red;
                        EditorGUI.LabelField(selectionRect, "Error!", _style);
                    }
                    else
                    {
                        _style.normal.textColor = Color.green;
                        EditorGUI.LabelField(selectionRect, elementType.generateType, _style);
                    }

                }
            }
        }
    }
}