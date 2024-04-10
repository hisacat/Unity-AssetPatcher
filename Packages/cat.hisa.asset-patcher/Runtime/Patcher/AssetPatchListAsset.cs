using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HisaCat.AssetPatcher.Patcher
{
    public class AssetPatchListAsset : ScriptableObject
    {
        public string AssetPatcherVersion { get => this.m_AssetPatcherVersion; set => this.m_AssetPatcherVersion = value; }
        [SerializeField] [ReadOnly] private string m_AssetPatcherVersion = null;

        public AssetPatchAsset[] AssetPatchAssets { get => this.m_assetPatchAssets; set => this.m_assetPatchAssets = value; }
        [SerializeField] [HideInInspector] private AssetPatchAsset[] m_assetPatchAssets = null;
    }
}
