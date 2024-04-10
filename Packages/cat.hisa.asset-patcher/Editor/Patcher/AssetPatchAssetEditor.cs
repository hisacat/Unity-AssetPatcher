using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HisaCat.AssetPatcher.Extensions;

namespace HisaCat.AssetPatcher.Patcher
{
    [CustomEditor(typeof(AssetPatchAsset))]
    [DisallowMultipleComponent]
    public class AssetPatchAssetEditor : Editor
    {
        private UnityEngine.Object originFile = null;
        private AssetPatchAsset asset = null;

        SerializedProperty m_AssetPatcherVersion = null;
        SerializedProperty m_originFileGUID = null;
        SerializedProperty m_diffFile = null;
        SerializedProperty m_metaFile = null;
        SerializedProperty m_outputGUID = null;
        SerializedProperty m_outputAssetPath = null;
        private void OnEnable()
        {
            this.asset = this.target as AssetPatchAsset;
            this.originFile = GetOriginFile(this.asset);

            this.m_AssetPatcherVersion = this.serializedObject.FindProperty(nameof(this.m_AssetPatcherVersion));
            this.m_originFileGUID = this.serializedObject.FindProperty(nameof(this.m_originFileGUID));
            this.m_diffFile = this.serializedObject.FindProperty(nameof(this.m_diffFile));
            this.m_metaFile = this.serializedObject.FindProperty(nameof(this.m_metaFile));
            this.m_outputGUID = this.serializedObject.FindProperty(nameof(this.m_outputGUID));
            this.m_outputAssetPath = this.serializedObject.FindProperty(nameof(this.m_outputAssetPath));
        }
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

                this.originFile = EditorGUILayout.ObjectField(GetLS("Window.OriginFile"), this.originFile, typeof(UnityEngine.Object), false);
                EditorGUILayout.PropertyField(this.m_originFileGUID, new GUIContent(GetLS("Window.OriginFileGUID")));
                EditorGUILayout.PropertyField(this.m_diffFile, new GUIContent(GetLS("Window.DiffFile")));
                EditorGUILayout.PropertyField(this.m_metaFile, new GUIContent(GetLS("Window.MetaFile")));
                EditorGUILayout.PropertyField(this.m_outputGUID, new GUIContent(GetLS("Window.OutputGUID")));
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.m_outputAssetPath, new GUIContent(GetLS("Window.OutputAssetPath")));
                bool doEditSelectOut = GUILayout.Button(GetLS("Window.EditOutputAssetPath"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
                if (doEditSelectOut)
                {
                    var name = System.IO.Path.GetFileNameWithoutExtension(this.m_outputAssetPath.stringValue);
                    var ext = System.IO.Path.GetExtension(this.m_outputAssetPath.stringValue).Replace(".", "");
                    var path = System.IO.Path.GetDirectoryName(this.m_outputAssetPath.stringValue);
                    var outputAssetPath = EditorUtility.SaveFilePanelInProject(GetLS("SaveFilePanelInProject.EditOutputAssetPath"), name, ext, GetLS("SaveFilePanelInProject.EditOutputAssetPath"), path);
                    if (string.IsNullOrEmpty(outputAssetPath) == false)
                        this.m_outputAssetPath.stringValue = outputAssetPath;
                }
                GUILayout.Space(25);
            }
            this.serializedObject.ApplyModifiedProperties();

            if (this.asset == null) return;
            if (this.originFile == null)
                EditorGUILayout.HelpBox(GetLS("Window.Error.OriginFileEmpty"), MessageType.Error);
            if (this.asset.DiffFile == null)
                EditorGUILayout.HelpBox(GetLS("Window.Error.DiffFileEmpty"), MessageType.Error);
            if (string.IsNullOrEmpty(this.asset.OutputAssetPath))
                EditorGUILayout.HelpBox(GetLS("Window.Error.OutputAssetPathEmpty"), MessageType.Error);

            EditorGUI.BeginDisabledGroup(this.originFile == null || this.asset.DiffFile == null || string.IsNullOrEmpty(this.asset.OutputAssetPath));
            {
                if (GUILayout.Button(GetLS("Window.Patch"), GUILayout.Height(50)))
                {
                    var outputAsset = AssetPatchManager.DoPatch(this.asset, originFile: this.originFile);
                    EditorGUIUtility.PingObject(outputAsset);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        public static UnityEngine.Object GetOriginFile(AssetPatchAsset asset)
        {
            if (string.IsNullOrEmpty(asset.OriginFileGUID))
                return null;
            return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(asset.OriginFileGUID));
        }
        public static void SetOriginFile(AssetPatchAsset asset, UnityEngine.Object originFile)
        {
            if (originFile == null)
            {
                asset.OriginFileGUID = null;
                return;
            }
            asset.OriginFileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(originFile));
        }
        private static string GetGlobalLS(string key) => I18n.GetLocalizedString($"Global.{key}");
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchAssetEditor.{key}");
    }
}
