using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO.Compression;

namespace HisaCat.AssetPatcher
{
    public static class ReleaseManager
    {
        private const string DirectoryKey = "HisaCat.AssetPatcher.ReleaseManager.Directory";
        public static string SaveDir
        {
            get => EditorPrefs.GetString(DirectoryKey);
            set => EditorPrefs.SetString(DirectoryKey, value);
        }

        public const string PackageManifestGUID = "08400e1c4c641334db85cb97c463bc1c";

        public readonly static string ReleaseFileName = $"cat.hisa.asset-patcher-v{Version.VersionStr}";

        [MenuItem("Tools/Release Manager/Release AssetPatcher")]
        public static void Release()
        {
            // Check version valid.
            var packageManifest = AssetDatabase.LoadAssetAtPath<UnityEditorInternal.PackageManifest>(AssetDatabase.GUIDToAssetPath(PackageManifestGUID));
            {
                var dict = Json.Deserialize(packageManifest.text) as Dictionary<string, object>;
                var packageVersion = dict["version"] as string;
                if (Version.VersionStr != packageVersion)
                {
                    Debug.LogError("Package Version and VersionStr not matched! Do not forgot edit \"CHANGES.md\", \"package.json\", \"Version.cs\"");
                    return;
                }
            }

            const string PackagesRootFolderAssetPath = "Packages/cat.hisa.asset-patcher";
            var packagesRootFolderIOPath = PathUtility.GetAssetIOPath(PackagesRootFolderAssetPath);
            if (System.IO.Directory.Exists(packagesRootFolderIOPath) == false)
                throw new System.Exception($"Root path not exists: \"{packagesRootFolderIOPath}\"");

            const string AssetRootFolderAssetPath = "Assets/HisaCat/VRC/AssetPatcher";
            var assetRootFolderIOPath = PathUtility.GetAssetIOPath(AssetRootFolderAssetPath);
            if (System.IO.Directory.Exists(assetRootFolderIOPath) == false)
                throw new System.Exception($"Root path not exists: \"{assetRootFolderIOPath}\"");

            var saveDir = EditorUtility.SaveFolderPanel($"Save Release (v{Version.VersionStr})", string.IsNullOrEmpty(SaveDir) ? null : System.IO.Path.GetDirectoryName(SaveDir), null);
            if (string.IsNullOrEmpty(saveDir)) return;
            SaveDir = saveDir;

            var zipPath = System.IO.Path.Combine(saveDir, $"{ReleaseFileName}.zip");
            var unitypackagePath = System.IO.Path.Combine(saveDir, $"{ReleaseFileName}.unitypackage");

            // Copy package.json
            var packageManifestIOPath = PathUtility.GetAssetIOPath(AssetDatabase.GetAssetPath(packageManifest));
            System.IO.File.Copy(packageManifestIOPath, System.IO.Path.Combine(saveDir, System.IO.Path.GetFileName(packageManifestIOPath)), true);

            // Create zip (VCC Package) release.
            {
                if (System.IO.File.Exists(zipPath))
                {
                    if (EditorUtility.DisplayDialog($"Save (v{Version.VersionStr})", "zip already exists. overwrite it?", "Ok") == false)
                        return;
                }
                System.IO.File.Delete(zipPath);

                ArchiveFilesInDirectoryAsZip(packagesRootFolderIOPath, zipPath);
            }

            // Create unitypackage release
            {
                if (System.IO.File.Exists(unitypackagePath))
                {
                    if (EditorUtility.DisplayDialog($"Save (v{Version.VersionStr})", "unitypackage already exists. overwrite it?", "Ok") == false)
                        return;
                }
                System.IO.File.Delete(unitypackagePath);

                void moveAllAssets(string from, string to)
                {
                    var fromIOPath = PathUtility.GetAssetIOPath(from);
                    var toOPath = PathUtility.GetAssetIOPath(to);

                    string[] files = System.IO.Directory.GetFiles(from);
                    foreach (string file in files)
                    {
                        string fileName = System.IO.Path.GetFileName(file);
                        if (fileName.StartsWith(".")) continue;

                        string destFile = System.IO.Path.Combine(toOPath, fileName);
                        if (System.IO.File.Exists(destFile))
                            throw new System.Exception($"File already exists: {destFile}");

                        System.IO.File.Move(file, destFile);
                    }

                    string[] folders = System.IO.Directory.GetDirectories(fromIOPath);
                    foreach (string folder in folders)
                    {
                        string folderName = System.IO.Path.GetFileName(folder);
                        string destFolder = System.IO.Path.Combine(toOPath, folderName);

                        if (System.IO.Directory.Exists(destFolder))
                            throw new System.Exception($"Directory already exists: {destFolder}");

                        System.IO.Directory.Move(folder, destFolder);
                    }
                    AssetDatabase.Refresh();
                }

                // Move files into Assets
                moveAllAssets(PackagesRootFolderAssetPath, AssetRootFolderAssetPath);

                // Export unitypackage
                AssetDatabase.ExportPackage(AssetRootFolderAssetPath, unitypackagePath, ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
                Debug.Log($"unitypackage exported: {unitypackagePath}");

                // Revert files that moved to Assets to the Package path.
                moveAllAssets(AssetRootFolderAssetPath, PackagesRootFolderAssetPath);

                // Copy Booth release files.
                {
                    var boothDir = System.IO.Path.Combine(saveDir, "Booth");
                    if (System.IO.Directory.Exists(boothDir) == false)
                        System.IO.Directory.CreateDirectory(boothDir);

                    // Copy unitypackage file
                    System.IO.File.Copy(unitypackagePath, System.IO.Path.Combine(boothDir, System.IO.Path.GetFileName(unitypackagePath)));
                    var boothCopyAssetPaths = new (string assetPath, string destPath)[]
                    {
                        ("Packages/cat.hisa.asset-patcher/CHANGELOG.md","CHANGELOG.txt"),
                        ("Packages/cat.hisa.asset-patcher/LICENSE","LICENSE.txt"),
                        ("Packages/cat.hisa.asset-patcher/README.md","README.txt"),
                    };

                    foreach (var copyAssetPath in boothCopyAssetPaths)
                    {
                        var destIOPath = copyAssetPath.destPath;
                        if (string.IsNullOrEmpty(destIOPath))
                            destIOPath = System.IO.Path.GetFileName(copyAssetPath.assetPath);
                        destIOPath = System.IO.Path.Combine(boothDir, destIOPath);

                        System.IO.File.Copy(PathUtility.GetAssetIOPath(copyAssetPath.assetPath), destIOPath, true);
                    }

                    ArchiveFilesInDirectoryAsZip(boothDir, System.IO.Path.Combine(boothDir, $"{ReleaseFileName}.zip"));
                }

                Debug.Log($"Release v{Version.VersionStr} exported.");
                System.Diagnostics.Process.Start(saveDir);
            }
        }

        private static void ArchiveFilesInDirectoryAsZip(string rootDirectoryPath, string zipPath)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var dirInfo = new System.IO.DirectoryInfo(rootDirectoryPath);
                    foreach (var file in dirInfo.GetFiles("*", System.IO.SearchOption.AllDirectories))
                    {
                        string pathInArchive = file.FullName.Substring(dirInfo.FullName.Length + 1);
                        var entry = archive.CreateEntry(pathInArchive);
                        using (var entryStream = entry.Open())
                        using (var fileStream = System.IO.File.OpenRead(file.FullName))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }

                using (var fileStream = new System.IO.FileStream(zipPath, System.IO.FileMode.Create))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
                Debug.Log($"zip exported: {zipPath}");
            }
        }
    }
}
