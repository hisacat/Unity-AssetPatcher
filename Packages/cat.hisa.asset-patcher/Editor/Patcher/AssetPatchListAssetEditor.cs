using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HisaCat.AssetPatcher.Extensions;

namespace HisaCat.AssetPatcher.Patcher
{
    [CustomEditor(typeof(AssetPatchListAsset))]
    [DisallowMultipleComponent]
    public class AssetPatchListAssetEditor : Editor
    {
        AssetPatchListAsset asset = null;

        SerializedProperty m_AssetPatcherVersion = null;
        SerializedProperty m_assetPatchAssets = null;
        private void OnEnable()
        {
            this.asset = this.target as AssetPatchListAsset;

            this.m_AssetPatcherVersion = this.serializedObject.FindProperty(nameof(this.m_AssetPatcherVersion));
            this.m_assetPatchAssets = this.serializedObject.FindProperty(nameof(this.m_assetPatchAssets));
        }

        private bool patchesFoldOut = true;
        public override void OnInspectorGUI()
        {
            ImageDrawer.DrawBanner();
            VersionDrawer.DrawVersion();

            I18n.DrawSelectLanguageGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            this.serializedObject.Update();
            {
                var centerLabelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(string.Format(GetGlobalLS("AssetPatcherVersionFormat"), this.m_AssetPatcherVersion.stringValue), centerLabelStyle);
                if (System.Version.TryParse(this.m_AssetPatcherVersion.stringValue, out var assetVersion) && System.Version.TryParse(Version.VersionStr, out var currentVersion))
                {
                    if (assetVersion > currentVersion)
                        EditorGUILayout.HelpBox(GetGlobalLS("HigherVersionWarningFormat").FormatNamed(new { currentVersion, assetVersion }), MessageType.Warning);
                }

                this.patchesFoldOut = EditorGUILayout.Foldout(patchesFoldOut, GetLS("Window.Patches"), true);
                if (patchesFoldOut)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    {
                        GUILayout.BeginVertical(new GUIStyle(GUI.skin.box));
                        EditorGUI.indentLevel++;
                        {
                            for (int i = 0; i < m_assetPatchAssets.arraySize; i++)
                            {
                                var patchAsset = this.m_assetPatchAssets.GetArrayElementAtIndex(i).objectReferenceValue;
                                EditorGUILayout.ObjectField($"{GetLS("Window.PatchAssets.Title")}{i}", patchAsset, typeof(AssetPatchAsset), false);
                            }
                        }
                        EditorGUI.indentLevel--;
                        GUILayout.EndVertical();
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            this.serializedObject.ApplyModifiedProperties();
            GUILayout.Space(25);

            if (GUILayout.Button(GetLS("Window.PatchAll"), GUILayout.Height(50)))
                AssetPatchManager.DoPatchAll(this.asset);
        }
        private static string GetGlobalLS(string key) => I18n.GetLocalizedString($"Global.{key}");
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchListAssetEditor.{key}");
    }
}
