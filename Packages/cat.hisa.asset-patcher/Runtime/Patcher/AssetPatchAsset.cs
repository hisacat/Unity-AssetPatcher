using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HisaCat.AssetPatcher.Patcher
{
    public class AssetPatchAsset : ScriptableObject
    {
        public string AssetPatcherVersion { get => this.m_AssetPatcherVersion; set => this.m_AssetPatcherVersion = value; }
        [SerializeField] [ReadOnly] private string m_AssetPatcherVersion = null;

        // Use GUID instead reference for prevent origin file becoming dependency of this asset.
        public string OriginFileGUID { get => this.m_originFileGUID; set => this.m_originFileGUID = value; }
        [SerializeField] [ReadOnly] private string m_originFileGUID = null;
        public TextAsset DiffFile { get => this.m_diffFile; set => this.m_diffFile = value; }
        [SerializeField] [ReadOnly] private TextAsset m_diffFile = null;
        public TextAsset MetaFile { get => this.m_metaFile; set => this.m_metaFile = value; }
        [SerializeField] [ReadOnly] private TextAsset m_metaFile = null;
        public string OutputGUID { get => this.m_outputGUID; set => this.m_outputGUID = value; }
        [SerializeField] [ReadOnly] private string m_outputGUID = null;
        public string OutputAssetPath { get => this.m_outputAssetPath; set => this.m_outputAssetPath = value; }
        [SerializeField] [ReadOnly] private string m_outputAssetPath = null;
    }
}
