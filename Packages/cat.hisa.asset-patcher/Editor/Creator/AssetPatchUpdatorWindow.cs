using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using HisaCat.AssetPatcher.Extensions;

namespace HisaCat.AssetPatcher.Creator
{
    public class AssetPatchUpdatorWindow : EditorWindow
    {
        [MenuItem("Tools/AssetPatcher/Update Asset Patch", priority = 0200)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<AssetPatchUpdatorWindow>();
            window.Show();
        }

        private SerializedObject so = null;
        private SerializedProperty patchAssetsProp = null;
        [SerializeField] private List<Patcher.AssetPatchAsset> patchAssets = new List<Patcher.AssetPatchAsset>();
        private ReorderableList patchAssetsReorderableList;
        private void OnEnable()
        {
            this.titleContent.text = "Update Asset Patch";
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

        //private Patcher.AssetPatchListAsset patchListAsset = null;
        private void OnGUI()
        {
            ImageDrawer.DrawBanner();
            VersionDrawer.DrawVersion();

            I18n.DrawSelectLanguageGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            var centerLabelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter };

            so.Update();
            {
                this.patchAssetsReorderableList.DoLayoutList();
            }
            so.ApplyModifiedProperties();

            EditorExtensions.GUILine();
            EditorGUILayout.LabelField(GetLS("Window.LoadFromList"), centerLabelStyle);
            var patchListAsset = EditorGUILayout.ObjectField(null, typeof(Patcher.AssetPatchListAsset), false) as Patcher.AssetPatchListAsset;
            if (patchListAsset != null)
            {
                var addedCount = 0;
                foreach (var patchAsset in patchListAsset.AssetPatchAssets)
                {
                    if (this.patchAssets.Contains(patchAsset) == false)
                    {
                        this.patchAssets.Add(patchAsset);
                        addedCount++;
                    }
                }

                EditorUtility.DisplayDialog(GetLS("Dialog.LoadFromList.Title"), string.Format(GetLS("Dialog.List.LoadFromList.DescFormat"), addedCount), GetLS("Dialog.LoadFromList.Ok"));
            }

            GUILayout.Space(25);
            var patchAssetsCount = this.patchAssets.Where(e => e != null).Count();
            if (patchAssetsCount <= 0)
                EditorGUILayout.HelpBox(GetLS("Window.PleaseSelectOneOrMorePatchFiles"), MessageType.Error);

            if (GUILayout.Button(GetLS("Window.UpdatePatches"), GUILayout.Height(50)))
            {
                int totalCount = 0;
                int successCount = 0;
                foreach (var patchAsset in this.patchAssets)
                {
                    if (patchAsset == null) continue;
                    totalCount++;

                    AssetPatchCreator.UpdateAssetPatch(patchAsset);
                    successCount++;
                }

                EditorUtility.DisplayDialog(GetLS("Dialog.Done.Title"), GetLS("Dialog.List.Done.DescFormat").FormatNamed(new { totalCount, successCount }), GetLS("Dialog.Done.Ok"));
            }
        }
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchUpdatorWindow.{key}");
    }
}
