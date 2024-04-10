using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;

namespace HisaCat.AssetPatcher.Creator
{
    public class CreateAssetPatchListWindow : EditorWindow
    {
        [MenuItem("Tools/AssetPatcher/Create Asset Patch List", priority = 0110)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<CreateAssetPatchListWindow>();
            window.Show();
        }

        private SerializedObject so = null;
        private SerializedProperty patchAssetsProp = null;
        [SerializeField] private List<Patcher.AssetPatchAsset> patchAssets = new List<Patcher.AssetPatchAsset>();
        private ReorderableList patchAssetsReorderableList;

        private string patchListAssetPath = null;
        private void OnEnable()
        {
            this.titleContent.text = "Create Asset Patch List";
            this.titleContent.image = ImageDrawer.WindowIconImage;

            ScriptableObject target = this;
            this.so = new SerializedObject(target);
            this.patchAssetsProp = so.FindProperty(nameof(this.patchAssets));
            this.patchAssetsReorderableList = new ReorderableList(this.so, this.patchAssetsProp, true, true, true, true);
            this.patchAssetsReorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, GetLS("Window.PatchAssets"));
            };
            this.patchAssetsReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = this.patchAssetsReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, GUIContent.none);
            };
        }
        private void OnGUI()
        {
            ImageDrawer.DrawBanner();
            VersionDrawer.DrawVersion();

            I18n.DrawSelectLanguageGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginChangeCheck();
            so.Update();
            {
                this.patchAssetsReorderableList.DoLayoutList();
            }
            so.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                // Remove duplicated assets
                var duplicatedAssets = this.patchAssets.Select((asset, i) => (asset, i))
                    .Where(e => e.asset != null)
                    .GroupBy(e => e.asset).Where(e => e.Count() > 1);
                foreach (var group in duplicatedAssets)
                {
                    for (int i = 1; i < group.Count(); i++)
                        this.patchAssets[group.ElementAt(i).i] = null;
                }
            }

            GUILayout.Space(25);
            var patchAssetsCount = this.patchAssets.Where(e => e != null).Count();
            if (patchAssetsCount <= 0)
                EditorGUILayout.HelpBox(GetLS("Window.PleaseSelectOneOrMorePatchFiles"), MessageType.Error);

            EditorGUI.BeginDisabledGroup(patchAssetsCount <= 0);
            {
                if (GUILayout.Button(GetLS("Window.CreatePatchListFile"), GUILayout.Height(50)))
                {
                    if (string.IsNullOrEmpty(this.patchListAssetPath)) this.patchListAssetPath = "Assets/";
                    this.patchListAssetPath = EditorUtility.SaveFilePanelInProject(GetLS("SaveFilePanelInProject.CreatePatchListFile"), "Patches", "asset", GetLS("SaveFilePanelInProject.SelectPatchListFilePath"), this.patchListAssetPath);
                    if (string.IsNullOrEmpty(this.patchListAssetPath)) return;

                    // Deselect asset if it selected for prevent past asset exposed in inspector when it overwritten.
                    if (Selection.activeObject == AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(this.patchListAssetPath))
                        Selection.activeObject = null;

                    var patchListAsset = AssetPatchCreator.CreateAssetPatchListAsset(this.patchAssets, this.patchListAssetPath);
                    EditorGUIUtility.PingObject(patchListAsset);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchListCreatorWindow.{key}");
    }
}
