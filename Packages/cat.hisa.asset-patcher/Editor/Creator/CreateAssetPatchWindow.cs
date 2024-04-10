using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher.Creator
{
    public class CreateAssetPatchWindow : EditorWindow
    {
        [MenuItem("Tools/AssetPatcher/Create Asset Patch", priority = 0100)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<CreateAssetPatchWindow>();
            window.Show();
        }

        private UnityEngine.Object originFile = null;
        private UnityEngine.Object editedFile = null;
        private bool saveMetaFile = true;
        private bool saveGUID = true;
        private void OnEnable()
        {
            this.titleContent.text = "Create Asset Patch";
            this.titleContent.image = ImageDrawer.WindowIconImage;
        }
        private void OnGUI()
        {
            ImageDrawer.DrawBanner();
            VersionDrawer.DrawVersion();

            I18n.DrawSelectLanguageGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            this.originFile = EditorGUILayout.ObjectField(GetLS("Window.OriginFile"), originFile, typeof(UnityEngine.Object), false);
            this.editedFile = EditorGUILayout.ObjectField(GetLS("Window.EditedFile"), editedFile, typeof(UnityEngine.Object), false);
            this.saveMetaFile = EditorGUILayout.Toggle(GetLS("Window.SaveMetaFile?"), saveMetaFile);
            EditorGUI.BeginDisabledGroup(this.saveMetaFile);
            this.saveGUID = EditorGUILayout.Toggle(GetLS("Window.KeepGUID?"), saveGUID);
            EditorGUI.EndDisabledGroup();
            if (this.saveMetaFile) this.saveGUID = true;
            if (this.originFile != null && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(this.originFile))) this.originFile = null;
            if (this.editedFile != null && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(this.editedFile))) this.editedFile = null;
            GUILayout.Space(25);

            if (!this.originFile)
                EditorGUILayout.HelpBox(GetLS("Window.PleaseSelectOriginFile"), MessageType.Error);
            else if (!this.editedFile)
                EditorGUILayout.HelpBox(GetLS("Window.PleaseSelectEditedFile"), MessageType.Error);

            EditorGUI.BeginDisabledGroup(this.originFile == null || this.editedFile == null);
            {
                if (GUILayout.Button(GetLS("Window.CreatePatchFile"), GUILayout.Height(50)))
                {
                    string patchAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(this.editedFile));
                    string patchAssetName = $"{System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(this.editedFile))} patch";
                    patchAssetPath = EditorUtility.SaveFilePanelInProject(GetLS("SaveFilePanelInProject.CreatePatchFile"), patchAssetName, "asset", GetLS("SaveFilePanelInProject.SelectPatchFilePath"), patchAssetPath);
                    if (string.IsNullOrEmpty(patchAssetPath)) return;

                    // Deselect asset if it selected for prevent past asset exposed in inspector when it overwritten.
                    if (Selection.activeObject == AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(patchAssetPath))
                        Selection.activeObject = null;

                    var assetPatchAsset = AssetPatchCreator.CreateAssetPatch(this.originFile, this.editedFile, this.saveMetaFile, this.saveGUID, patchAssetPath);
                    EditorGUIUtility.PingObject(assetPatchAsset);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchCreatorWindow.{key}");
    }
}
