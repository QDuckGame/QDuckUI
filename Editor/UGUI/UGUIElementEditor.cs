using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace QDuck.UI.UGUI
{

    [CustomEditor(typeof(UGUIElement))]
    public class UGUIElementEditor : Editor
    {
        private string[] componentTypes;
        private int selectedIndex;

        private void OnEnable()
        {
            UGUIElement element = (UGUIElement)target;
            componentTypes = element.GetComponents<Component>()
                .Where(c => c.GetType() != typeof(UGUIElement))
                .Select(c => c.GetType().Name)
                .ToArray();
            selectedIndex = componentTypes.Length - 1; // Set the last component as the default selected element
        }

        public override void OnInspectorGUI()
        {
            UGUIElement element = (UGUIElement)target;

            if (componentTypes.Length > 0)
            {
                selectedIndex = EditorGUILayout.Popup("Component Type", selectedIndex, componentTypes);
                element.generateType = componentTypes[selectedIndex];
            }
            else
            {
                EditorGUILayout.LabelField("No components found on this GameObject.");
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}