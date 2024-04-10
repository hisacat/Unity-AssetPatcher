using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher
{
    public static class VersionDrawer
    {
        public static void DrawVersion()
        {
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField($"Version: v{Version.VersionStr}", style, GUILayout.ExpandWidth(true));
        }
    }
}
