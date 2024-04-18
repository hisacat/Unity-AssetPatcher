using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher
{

    public static class AssetDatabaseUtility
    {
        public static bool IsAssetExists(string assetPath) => AssetDatabase.GetMainAssetTypeAtPath(assetPath) != null;
    }
}
