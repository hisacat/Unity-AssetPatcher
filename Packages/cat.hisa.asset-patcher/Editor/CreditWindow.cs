using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher
{
    public class CreditWindow : EditorWindow
    {
        [MenuItem("Tools/AssetPatcher/Credit", priority = 9000)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<CreditWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            this.titleContent.text = "AssetPatcher - Credit";
            this.titleContent.image = ImageDrawer.WindowIconImage;
            this.minSize = new Vector2(280, 250);
        }

        private Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {
            ImageDrawer.DrawBanner();

            var centerLabelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter };
            var centerBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            var bigCenterLabelStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 16 };

            EditorGUILayout.LabelField("AssetPatcher", bigCenterLabelStyle);
            VersionDrawer.DrawVersion();
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("GitHub", EditorStyles.linkLabel)) Application.OpenURL("https://github.com/hisacat/VPM-AssetPatcher");
                EditorGUILayout.LabelField("|", centerLabelStyle, GUILayout.Width(10));
                if (GUILayout.Button("Booth", EditorStyles.linkLabel)) Application.OpenURL("https://hisacat.booth.pm/items/5612806");
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorExtensions.GUILine();

            this.scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                var height = 128;
                var width = ImageDrawer.HisaCatImage.width * height / ImageDrawer.HisaCatImage.height;
                GUI.DrawTexture(GUILayoutUtility.GetRect(width, height), ImageDrawer.HisaCatImage, ScaleMode.ScaleToFit);
                GUILayout.Space(12);
                EditorGUILayout.LabelField("Developed by HisaCat", bigCenterLabelStyle);
                EditorGUILayout.LabelField("ahisacat@gmail.com", centerLabelStyle);

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUILayout.LabelField("Contacts", centerBoldLabelStyle);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("GitHub", EditorStyles.linkLabel)) Application.OpenURL("https://github.com/hisacat");
                    EditorGUILayout.LabelField("|", centerLabelStyle, GUILayout.Width(10));
                    if (GUILayout.Button("Booth", EditorStyles.linkLabel)) Application.OpenURL("https://hisacat.booth.pm");
                    EditorGUILayout.LabelField("|", centerLabelStyle, GUILayout.Width(10));
                    if (GUILayout.Button("Twitter", EditorStyles.linkLabel)) Application.OpenURL("https://twitter.com/ahisacat");
                    EditorGUILayout.LabelField("|", centerLabelStyle, GUILayout.Width(10));
                    if (GUILayout.Button("Discord", EditorStyles.linkLabel)) Application.OpenURL("https://discord.com/users/295816286213242880");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorExtensions.GUILine();

                EditorGUILayout.LabelField("Credit", centerBoldLabelStyle);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("sisong's HDiffPatch", EditorStyles.linkLabel)) Application.OpenURL("https://github.com/sisong/HDiffPatch");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorExtensions.GUILine();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
