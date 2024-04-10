using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace HisaCat.AssetPatcher.Creator
{
    public static class AssetPatchCreator
    {
        public static Patcher.AssetPatchAsset CreateAssetPatch(UnityEngine.Object oldFile, UnityEngine.Object editedFile, bool saveMetaFile, bool saveGUID, string patchAssetPath)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
                throw new System.PlatformNotSupportedException("Only works in Windows!");

            if (saveMetaFile) saveGUID = true;

            var newAssetPath = AssetDatabase.GetAssetPath(editedFile);

            // Create AssetPatchAsset.
            var assetPatchAsset = ScriptableObject.CreateInstance<Patcher.AssetPatchAsset>();
            assetPatchAsset.AssetPatcherVersion = Version.VersionStr;

            Patcher.AssetPatchAssetEditor.SetOriginFile(assetPatchAsset, oldFile);
            assetPatchAsset.OutputAssetPath = newAssetPath;
            assetPatchAsset.OutputGUID = saveGUID ? AssetDatabase.AssetPathToGUID(newAssetPath) : null;
            AssetDatabase.CreateAsset(assetPatchAsset, patchAssetPath);
            AssetDatabase.Refresh();

            GenerateDataFiles(oldFile, editedFile, saveMetaFile, patchAssetPath, out var diffAssetPath, out var savedMetaAssetPath);

            // Assign patch assets into AssetPatchAsset.
            var diffAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(diffAssetPath);
            var savedMetaAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(savedMetaAssetPath);

            assetPatchAsset.DiffFile = diffAsset;
            if (saveMetaFile) assetPatchAsset.MetaFile = savedMetaAsset;
            EditorUtility.SetDirty(assetPatchAsset);
            AssetDatabase.SaveAssets();

            Debug.Log($"<color=green><b>Success to create patch asset:</b></color> \"{patchAssetPath}\"", assetPatchAsset);

            return assetPatchAsset;
        }
        public static Patcher.AssetPatchListAsset CreateAssetPatchListAsset(IEnumerable<Patcher.AssetPatchAsset> patchAssets, string patchListAssetPath)
        {
            // Create AssetPatchListAsset
            var patchListAsset = ScriptableObject.CreateInstance<Patcher.AssetPatchListAsset>();
            patchListAsset.AssetPatcherVersion = Version.VersionStr;

            patchListAsset.AssetPatchAssets = patchAssets.Where(e => e != null).Distinct().ToArray();
            AssetDatabase.CreateAsset(patchListAsset, patchListAssetPath);
            AssetDatabase.Refresh();

            Debug.Log($"<color=green><b>Success to create patch list asset:</b></color> \"{patchListAssetPath}\"", patchListAsset);

            return patchListAsset;
        }
        public static void UpdateAssetPatch(Patcher.AssetPatchAsset assetPatchAsset)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
                throw new System.PlatformNotSupportedException("Only works in Windows!");

            // Validate patch asset.
            var originFile = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(assetPatchAsset.OriginFileGUID));
            if (originFile == null) throw new System.Exception("Cannot find origin file.");
            var editedFile = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPatchAsset.OutputAssetPath);
            if (editedFile == null) throw new System.Exception("Cannot find edited file.");

            // Parse options.
            var saveMetaFile = assetPatchAsset.MetaFile != null;
            var patchAssetPath = AssetDatabase.GetAssetPath(assetPatchAsset);

            // Remove old datas.
            {
                var oldDiffFileAssetPath = assetPatchAsset.DiffFile == null ? null : AssetDatabase.GetAssetPath(assetPatchAsset.DiffFile);
                var oldMetaFileAssetPath = assetPatchAsset.MetaFile == null ? null : AssetDatabase.GetAssetPath(assetPatchAsset.MetaFile);
                if (string.IsNullOrEmpty(oldDiffFileAssetPath) == false) AssetDatabase.DeleteAsset(oldDiffFileAssetPath);
                if (string.IsNullOrEmpty(oldMetaFileAssetPath) == false) AssetDatabase.DeleteAsset(oldMetaFileAssetPath);
                AssetDatabase.Refresh();

                void removeDirectoryIfEmpty(string assetPath)
                {
                    if (string.IsNullOrEmpty(assetPath)) return;
                    var ioPath = PathUtility.GetAssetIOPath(assetPath);
                    if (System.IO.Directory.Exists(ioPath) == false) return;

                    var files = System.IO.Directory.GetFiles(ioPath);
                    if (files == null || files.Length <= 0)
                    {
                        AssetDatabase.DeleteAsset(assetPath);
                        AssetDatabase.Refresh();
                    }
                }

                removeDirectoryIfEmpty(oldDiffFileAssetPath);
                removeDirectoryIfEmpty(oldMetaFileAssetPath);
            }

            GenerateDataFiles(originFile, editedFile, saveMetaFile, patchAssetPath, out var diffAssetPath, out var savedMetaAssetPath);

            assetPatchAsset.AssetPatcherVersion = Version.VersionStr;
            assetPatchAsset.DiffFile = AssetDatabase.LoadAssetAtPath<TextAsset>(diffAssetPath);
            assetPatchAsset.MetaFile = saveMetaFile ? AssetDatabase.LoadAssetAtPath<TextAsset>(savedMetaAssetPath) : null;
            EditorUtility.SetDirty(assetPatchAsset);
            AssetDatabase.SaveAssets();

            Debug.Log($"<color=green><b>Success to update patch asset:</b></color> \"{patchAssetPath}\"", assetPatchAsset);
        }

        public static void GenerateDataFiles(UnityEngine.Object oldFile, UnityEngine.Object editedFile, bool saveMetaFile, string patchAssetPath, out string diffAssetPath, out string savedMetaAssetPath)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
                throw new System.PlatformNotSupportedException("Only works in Windows!");

            var (oldAssetPath, newAssetPath) = (AssetDatabase.GetAssetPath(oldFile), AssetDatabase.GetAssetPath(editedFile));
            var (oldAssetIOPath, newAssetIOPath) = (PathUtility.GetAssetIOPath(oldAssetPath), PathUtility.GetAssetIOPath(newAssetPath));
            var newMetaAssetPath = $"{newAssetPath}.meta";
            var newMetaIOPath = PathUtility.GetAssetIOPath(newMetaAssetPath);

            // Create datas directory.
            string datasDirAssetPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(patchAssetPath), $"{System.IO.Path.GetFileNameWithoutExtension(patchAssetPath)}-datas");
            string datasDirIOPath = PathUtility.GetAssetIOPath(datasDirAssetPath);

            if (System.IO.Directory.Exists(datasDirIOPath)) System.IO.Directory.Delete(datasDirIOPath, true);
            System.IO.Directory.CreateDirectory(datasDirIOPath);
            AssetDatabase.Refresh();

            // Cretae Diff file.
            diffAssetPath = System.IO.Path.Combine(datasDirAssetPath, $"diff.bytes");
            string diffIOPath = PathUtility.GetAssetIOPath(diffAssetPath);

            EditorUtility.DisplayProgressBar(GetLS("Progress.CreatePatchFile"), GetLS("Progress.Working"), 0f);
            try
            {
                Patcher.HDiffPatchWrapper.hdiffz(oldAssetIOPath, newAssetIOPath, diffIOPath);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            EditorUtility.DisplayProgressBar(GetLS("Progress.CreatePatchFile"), GetLS("Progress.Working"), 1f);
            EditorUtility.ClearProgressBar();

            if (System.IO.File.Exists(diffIOPath) == false)
            {
                AssetDatabase.DeleteAsset(patchAssetPath);
                AssetDatabase.DeleteAsset(datasDirAssetPath);
                AssetDatabase.Refresh();
                throw new System.Exception("Failed to create patch file. See log.");
            }

            // Save Meta file.
            savedMetaAssetPath = System.IO.Path.Combine(datasDirAssetPath, $"meta.bytes");
            string savedMetaIOPath = PathUtility.GetAssetIOPath(savedMetaAssetPath);

            if (saveMetaFile)
            {
                System.IO.File.Copy(newMetaIOPath, savedMetaIOPath, overwrite: true);
                if (System.IO.File.Exists(savedMetaIOPath) == false)
                {
                    AssetDatabase.DeleteAsset(patchAssetPath);
                    AssetDatabase.DeleteAsset(datasDirAssetPath);
                    AssetDatabase.Refresh();
                    throw new System.Exception("Failed to save meta file.");
                }
            }

            // Refresh AssetDatabase for Diff and Meta files.
            AssetDatabase.Refresh();
        }
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchCreator.{key}");
    }
}
