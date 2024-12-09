using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace QDuck.UI.UGUI
{

    [CustomEditor(typeof(UGUIComponent))]
    public class UGUIElementEditor : Editor
    {
        private string[] componentTypes;
        private int selectedIndex;

        private void OnEnable()
        {
            UGUIComponent component = (UGUIComponent)target;
            componentTypes = component.GetComponents<Component>()
                .Where(c => c.GetType() != typeof(UGUIComponent))
                .Select(c => c.GetType().Name)
                .ToArray();
            selectedIndex = componentTypes.Length - 1; // Set the last component as the default selected element
        }

        public override void OnInspectorGUI()
        {
            UGUIComponent component = (UGUIComponent)target;

            if (componentTypes.Length > 0)
            {
                selectedIndex = EditorGUILayout.Popup("Component Type", selectedIndex, componentTypes);
                component.generateType = componentTypes[selectedIndex];
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