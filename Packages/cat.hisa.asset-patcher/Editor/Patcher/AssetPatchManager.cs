using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HisaCat.AssetPatcher.Extensions;

namespace HisaCat.AssetPatcher.Patcher
{
    public static class AssetPatchManager
    {
        public static UnityEngine.Object DoPatch(AssetPatchAsset asset, UnityEngine.Object originFile = null, bool doSilent = false)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
                throw new System.PlatformNotSupportedException("Only works in Windows!");

            // Pre-Validate patchable.
            {
                var invalidReason = ValidatePatchable(asset, originFile);
                switch (invalidReason)
                {
                    case InvalidPatchAssetReason.OriginFileMissing:
                        EditorUtility.DisplayDialog(GetLS("Dialog.Error.Title"), GetLS("Dialog.Error.Desc.OriginFileMissing"), GetLS("Dialog.Error.Ok"));
                        EditorGUIUtility.PingObject(asset);
                        throw new System.Exception("Origin File Missing!");

                    case InvalidPatchAssetReason.DiffFileMissing:
                        EditorUtility.DisplayDialog(GetLS("Dialog.Error.Title"), GetLS("Dialog.Error.Desc.DiffFileMissing"), GetLS("Dialog.Error.Ok"));
                        EditorGUIUtility.PingObject(asset);
                        throw new System.Exception("Diff File Missing!");

                    default:
                        break;
                }
            }

            // Ask overwrite same GUID's asset instead output path if it same GUID's asset existing.
            string outputAssetPath = asset.OutputAssetPath;
            bool doOverwriteSameGUIDOutputAsset = false;
            if (string.IsNullOrEmpty(asset.OutputGUID) == false)
            {
                var existingOutputAssetPath = AssetDatabase.GUIDToAssetPath(asset.OutputGUID);
                if (existingOutputAssetPath != null)
                {
                    if (existingOutputAssetPath != asset.OutputAssetPath)
                    {
                        if (EditorUtility.DisplayDialog(GetLS("Dialog.overwriteSameGUIDOutputAsset.Title"),
                            string.Format(GetLS("Dialog.overwriteSameGUIDOutputAsset.DescFormat"), existingOutputAssetPath),
                            GetLS("Dialog.overwriteSameGUIDOutputAsset.Yes"), GetLS("Dialog.overwriteSameGUIDOutputAsset.No")))
                        {
                            doOverwriteSameGUIDOutputAsset = true;
                            outputAssetPath = existingOutputAssetPath;
                        }
                    }
                }
            }

            // Ask overwrite if output asset already exists.
            string outputIOPath = PathUtility.GetAssetIOPath(outputAssetPath);
            if (doOverwriteSameGUIDOutputAsset == false)
            {
                if (System.IO.File.Exists(outputIOPath))
                {
                    if (EditorUtility.DisplayDialog(GetLS("Dialog.AlreadyExists.Title"),
                        string.Format(GetLS("Dialog.AlreadyExists.DescFormat"), outputAssetPath),
                        GetLS("Dialog.AlreadyExists.Yes"), GetLS("Dialog.AlreadyExists.No")) == false)
                        return null;
                }
            }

            if (originFile == null) originFile = AssetPatchAssetEditor.GetOriginFile(asset);

            // Validate patchable.
            {
                var invalidReason = ValidatePatchable(asset, originFile);
                switch (invalidReason)
                {
                    case InvalidPatchAssetReason.OutputAssetPathMissing:
                        EditorUtility.DisplayDialog(GetLS("Dialog.Error.Title"), GetLS("Dialog.Error.Desc.OutputAssetPathMissing"), GetLS("Dialog.Error.Ok"));
                        EditorGUIUtility.PingObject(asset);
                        throw new System.Exception("Output Asset Path Missing!");

                    default:
                        break;
                }
            }

            // Patch file.
            string originIOPath = PathUtility.GetAssetIOPath(AssetDatabase.GetAssetPath(originFile));
            string diffIOPath = PathUtility.GetAssetIOPath(AssetDatabase.GetAssetPath(asset.DiffFile));

            try
            {
                HDiffPatchWrapper.hpatchz(originIOPath, diffIOPath, outputIOPath);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            if (System.IO.File.Exists(outputIOPath) == false)
                throw new System.Exception("Failed to patch. See log.");

            // Copy meta file.
            if (asset.MetaFile != null)
            {
                string metaAssetPath = AssetDatabase.GetAssetPath(asset.MetaFile);
                string metaIOPath = PathUtility.GetAssetIOPath(metaAssetPath);
                System.IO.File.Copy(metaIOPath, System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(outputIOPath),
                    $"{System.IO.Path.GetFileName(outputIOPath)}.meta"), overwrite: true);
            }

            // Refresh AssetDataBase for Output file.
            AssetDatabase.Refresh();

            // Overwrite GUID.
            if (string.IsNullOrEmpty(asset.OutputGUID) == false)
            {
                var oldGUID = AssetDatabase.AssetPathToGUID(outputAssetPath);
                var newGUID = asset.OutputGUID;
                if (oldGUID != newGUID)
                {
                    string metaPath = $"{outputAssetPath}.meta";
                    if (System.IO.File.Exists(metaPath) == false)
                    {
                        Debug.LogError($"Failed to overwrite GUID. meta file not exists: \"{outputAssetPath}\"");
                    }
                    else
                    {
                        string metaContent = System.IO.File.ReadAllText(metaPath);
                        metaContent = metaContent.Replace(oldGUID, newGUID);
                        System.IO.File.WriteAllText(metaPath, metaContent);
                        AssetDatabase.Refresh();
                    }
                }
            }

            if (!doSilent)
                EditorUtility.DisplayDialog(GetLS("Dialog.Done.Title"), GetLS("Dialog.Done.Desc"), GetLS("Dialog.Done.Ok"));

            Debug.Log($"<color=green><b>Success to patch:</b></color> \"{outputAssetPath}\"", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(outputAssetPath));

            var outputAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(outputAssetPath);
            return outputAsset;
        }

        public static void DoPatchAll(IEnumerable<AssetPatchAsset> assets, bool doSilent = false)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
                throw new System.PlatformNotSupportedException("Only works in Windows!");

            int totalCount = 0;
            int successCount = 0;
            if (assets != null)
            {
                foreach (var asset in assets)
                {
                    totalCount++;
                    try
                    {
                        if (asset != null) DoPatch(asset, doSilent: true);
                        successCount++;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e, asset);
                        Debug.LogError($"Failed to patch asset \"{AssetDatabase.GetAssetPath(asset)}\"", asset);
                    }
                }
            }

            if (!doSilent)
                EditorUtility.DisplayDialog(GetLS("Dialog.Done.Title"), GetLS("Dialog.List.Done.DescFormat").FormatNamed(new { totalCount, successCount }), GetLS("Dialog.Done.Ok"));
        }
        public static void DoPatchAll(AssetPatchListAsset asset, bool doSilent = false) => DoPatchAll(asset.AssetPatchAssets, doSilent);

        public enum InvalidPatchAssetReason : int
        {
            None = 0,
            OriginFileMissing = 1,
            DiffFileMissing = 2,
            OutputAssetPathMissing = 3,
        }

        public static InvalidPatchAssetReason ValidatePatchable(AssetPatchAsset asset, UnityEngine.Object originFile = null)
        {
            if (originFile == null) originFile = AssetPatchAssetEditor.GetOriginFile(asset);
            if (originFile == null) return InvalidPatchAssetReason.OriginFileMissing;
            if (asset.DiffFile == null) return InvalidPatchAssetReason.DiffFileMissing;
            if (string.IsNullOrEmpty(asset.OutputAssetPath)) return InvalidPatchAssetReason.OutputAssetPathMissing;
            return InvalidPatchAssetReason.None;
        }
        private static string GetLS(string key) => I18n.GetLocalizedString($"AssetPatchManager.{key}");
    }
}
