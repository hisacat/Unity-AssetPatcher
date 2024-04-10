using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher
{
    public static class ImageDrawer
    {
        public static string BannerImageGUID_DarkTheme = "6ce2572fcc282f44ba137982ab143de9";
        public static string BannerImageGUID_LightTheme = "31af103937d7d99409be29b133289b4d";
        public readonly static Texture2D BannerImage;
        public static string assetIconImageGUID = "8a76d5c99878f484f9f91de8f64910b6";
        public readonly static Texture2D AssetIconImage;
        public static string windowIconImageGUID = "1763007e8e49eff4b9cad2fca1c2b51d";
        public readonly static Texture2D WindowIconImage;
        public static string hisaCatImageGUID = "7a290b3f8976bf84da56b4f48aa916bc";
        public readonly static Texture2D HisaCatImage;

        static ImageDrawer()
        {
            var bannerImageGUID = EditorGUIUtility.isProSkin ? BannerImageGUID_DarkTheme : BannerImageGUID_LightTheme;
            BannerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(bannerImageGUID));
            AssetIconImage = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(assetIconImageGUID));
            WindowIconImage = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(windowIconImageGUID));
            HisaCatImage = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(hisaCatImageGUID));
        }

        public static void DrawBanner()
        {
            if (BannerImage == null) return;

            GUILayout.Space(EditorStyles.label.lineHeight);
            var height = EditorStyles.label.lineHeight * 3;
            var width = BannerImage.width * height / BannerImage.height;
            GUI.DrawTexture(GUILayoutUtility.GetRect(width, height), BannerImage, ScaleMode.ScaleToFit);
            GUILayout.Space(EditorStyles.label.lineHeight);
        }
    }
}
