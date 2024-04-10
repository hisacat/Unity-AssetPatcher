using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher
{
    public static class PathUtility
    {
        public const string DependencyPath = "Assets/HisaCat/VRC/AssetPatcher/Patcher/Dependency";
        public const string DependencyFolderGUID = "505931585d8a77a499797afb41269d82";

        public static string GetProjectPath() => System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, "../"));
        public static string GetAssetIOPath(string assetPath) => System.IO.Path.Combine(GetProjectPath(), assetPath);
        public static string GetDependencyPath()
        {
            string dependencyPath = UnityEditor.AssetDatabase.GUIDToAssetPath(DependencyFolderGUID);
            if (string.IsNullOrEmpty(dependencyPath))
            {
                Debug.LogWarning($"Cannot find Dependency folder. fallback to static path: \"{DependencyPath}\"");
                dependencyPath = DependencyPath;
            }

            var path = System.IO.Path.Combine(GetProjectPath(), dependencyPath);
            if (System.IO.Directory.Exists(path) == false)
                throw new System.Exception("Cannot find Dependency directory");

            return path;
        }
        public static string GethdiffpatchDirPath() => System.IO.Path.Combine(GetDependencyPath(), "hdiffpatch/windows32");
        public static string GethdiffzPath() => System.IO.Path.Combine(GethdiffpatchDirPath(), "hdiffz.exe");
        public static string GethpatchzPath() => System.IO.Path.Combine(GethdiffpatchDirPath(), "hpatchz.exe");
    }
}
