using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher.Patcher
{
    [UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
            => UnityEditor.EditorGUI.GetPropertyHeight(property, label, true);

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            UnityEditor.EditorGUI.PropertyField(position, property, label, true);
            UnityEditor.EditorGUI.EndDisabledGroup();
        }
    }
}
