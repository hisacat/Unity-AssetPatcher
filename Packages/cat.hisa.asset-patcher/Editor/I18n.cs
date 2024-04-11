using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HisaCat.AssetPatcher
{
    public static class I18n
    {
        public enum Language : int
        {
            English,
            Japanese,
            Korean,
        }
        public const Language DefaultLanguage = Language.English;

        private const string CurrentLanguageKey = "HisaCat.AssetPatcher.I18n.CurrentLanguage";
        public static Language CurrentLanguage
        {
            get
            {
                // Use and set system language when initial loaded.
                if (EditorPrefs.HasKey(CurrentLanguageKey) == false)
                {
                    switch (Application.systemLanguage)
                    {
                        case SystemLanguage.Japanese:
                            return CurrentLanguage = Language.Japanese;
                        case SystemLanguage.Korean:
                            return CurrentLanguage = Language.Korean;
                        default:
                            return CurrentLanguage = DefaultLanguage;
                    }
                }

                var langStr = EditorPrefs.GetString(CurrentLanguageKey);
                if (System.Enum.TryParse<Language>(langStr, out var lang))
                    return lang;
                return DefaultLanguage;
            }
            set => EditorPrefs.SetString(CurrentLanguageKey, value.ToString());
        }

        public static void DrawSelectLanguageGUI()
            => CurrentLanguage = (Language)EditorGUILayout.Popup("Language:", (int)CurrentLanguage, new string[] { "English", "日本語", "한국어" });

        public static string GetLocalizedString(string key) => GetLocalizedString(key, CurrentLanguage);
        public static string GetLocalizedString(string key, Language language)
        {
            switch (key)
            {
                #region AssetPatchCreatorWindow
                case "AssetPatchCreatorWindow.Window.OriginFile":
                    switch (language)
                    {
                        case Language.English: return "Origin File";
                        case Language.Japanese: return "元のファイル";
                        case Language.Korean: return "원본 파일";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.Window.EditedFile":
                    switch (language)
                    {
                        case Language.English: return "Edited File";
                        case Language.Japanese: return "修正後ファイル";
                        case Language.Korean: return "수정된 파일";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.Window.SaveMetaFile?":
                    switch (language)
                    {
                        case Language.English: return "Save meta file?";
                        case Language.Japanese: return "metaファイル保存";
                        case Language.Korean: return "meta 파일 저장하기";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.Window.KeepGUID?":
                    switch (language)
                    {
                        case Language.English: return "Keep GUID?";
                        case Language.Japanese: return "GUID保存";
                        case Language.Korean: return "GUID 보존";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.Window.PleaseSelectOriginFile":
                    switch (language)
                    {
                        case Language.English: return "Please select \"Origin File\".";
                        case Language.Japanese: return "「元のファイル」を選択してください。";
                        case Language.Korean: return "\"원본 파일\"을 선택해 주세요.";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.Window.PleaseSelectEditedFile":
                    switch (language)
                    {
                        case Language.English: return "Please select \"Edited File\".";
                        case Language.Japanese: return "「修正後ファイル」を選択してください。";
                        case Language.Korean: return "\"수정된 파일\"을 선택해 주세요.";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.Window.CreatePatchFile":
                    switch (language)
                    {
                        case Language.English: return "Create Patch File";
                        case Language.Japanese: return "パッチファイルの作成";
                        case Language.Korean: return "패치 파일 만들기";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.SaveFilePanelInProject.CreatePatchFile":
                    switch (language)
                    {
                        case Language.English: return "Create Patch File";
                        case Language.Japanese: return "パッチファイルの作成";
                        case Language.Korean: return "패치 파일 만들기";
                        default: return key;
                    }
                case "AssetPatchCreatorWindow.SaveFilePanelInProject.SelectPatchFilePath":
                    switch (language)
                    {
                        case Language.English: return "Select patch file path";
                        case Language.Japanese: return "パッチファイルのパス選択";
                        case Language.Korean: return "패치 파일 경로 선택";
                        default: return key;
                    }
                #endregion AssetPatchCreatorWindow

                #region AssetPatchListCreatorWindow
                case "AssetPatchListCreatorWindow.Window.PatchAssets":
                    switch (language)
                    {
                        case Language.English: return "Patches";
                        case Language.Japanese: return "パッチファイルのリスト";
                        case Language.Korean: return "패치 파일 목록";
                        default: return key;
                    }
                case "AssetPatchListCreatorWindow.Window.PleaseSelectOneOrMorePatchFiles":
                    switch (language)
                    {
                        case Language.English: return "Please select one or more patch files.";
                        case Language.Japanese: return "1つ以上のパッチファイルを選択してください。";
                        case Language.Korean: return "한 개 이상의 패치 파일을 선택해 주세요.";
                        default: return key;
                    }
                case "AssetPatchListCreatorWindow.Window.CreatePatchListFile":
                    switch (language)
                    {
                        case Language.English: return "Create Patch List File";
                        case Language.Japanese: return "パッチリストファイルの作成";
                        case Language.Korean: return "패치 리스트 파일 만들기";
                        default: return key;
                    }
                case "AssetPatchListCreatorWindow.SaveFilePanelInProject.CreatePatchListFile":
                    switch (language)
                    {
                        case Language.English: return "Create Patch List File";
                        case Language.Japanese: return "パッチリストファイルの作成";
                        case Language.Korean: return "패치 리스트 파일 만들기";
                        default: return key;
                    }
                case "AssetPatchListCreatorWindow.SaveFilePanelInProject.SelectPatchListFilePath":
                    switch (language)
                    {
                        case Language.English: return "Select patch list file path";
                        case Language.Japanese: return "パッチリストファイルのパス選択";
                        case Language.Korean: return "패치 리스트 파일 경로 선택";
                        default: return key;
                    }
                #endregion AssetPatchListCreatorWindow

                #region Global
                case "Global.AssetPatcherVersionFormat":
                    switch (language)
                    {
                        case Language.English: return "This patch file was created with AssetPatcher v{0}.";
                        case Language.Japanese: return "このパッチ ファイルは AssetPatcher v{0} で作成されました。";
                        case Language.Korean: return "이 패치 파일은 AssetPatcher v{0}로 생성되었습니다.";
                        default: return key;
                    }
                case "Global.HigherVersionWarningFormat":
                    switch (language)
                    {
                        case Language.English:
                            return "This patch asset created at a version (v{assetVersion}) higher than the current version (v{currentVersion})." +
                                "\r\nThe patch may not work properly." +
                                "\r\nPlease update AssetPatcher.";
                        case Language.Japanese:
                            return "現在のバージョン（v{currentVersion}）より高いバージョン（v{assetVersion}）で作成されたパッチアセットです。" +
                                "\r\nパッチが正常に行われない場合には" +
                                "\r\nAssetPatcherをアップデートしてください。";
                        case Language.Korean:
                            return "현재 버전(v{currentVersion})보다 높은 버전(v{assetVersion})에서 작성된 패치 어셋입니다." +
                                "\r\n패치가 정상적으로 이루어지지 않을 수 있습니다." +
                                "\r\nAssetPatcher를 업데이트 해 주세요.";
                        default: return key;
                    }
                #endregion Global

                #region AssetPatchAssetEditor
                case "AssetPatchAssetEditor.Window.OriginFile":
                    switch (language)
                    {
                        case Language.English: return "Origin File";
                        case Language.Japanese: return "元のファイル";
                        case Language.Korean: return "원본 파일";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.OriginFileGUID":
                    switch (language)
                    {
                        case Language.English: return "Origin File GUID";
                        case Language.Japanese: return "元のファイルGUID";
                        case Language.Korean: return "원본 파일 GUID";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.DiffFile":
                    switch (language)
                    {
                        case Language.English: return "Diff File";
                        case Language.Japanese: return "差分ファイル";
                        case Language.Korean: return "차이 파일";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.MetaFile":
                    switch (language)
                    {
                        case Language.English: return "Meta File";
                        case Language.Japanese: return "Metaファイル";
                        case Language.Korean: return "Meta 파일";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.OutputGUID":
                    switch (language)
                    {
                        case Language.English: return "Output GUID";
                        case Language.Japanese: return "エクスポートGUID";
                        case Language.Korean: return "내보내기 GUID";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.OutputAssetPath":
                    switch (language)
                    {
                        case Language.English: return "Output Asset Path";
                        case Language.Japanese: return "エクスポートパス";
                        case Language.Korean: return "내보내기 경로";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.EditOutputAssetPath":
                    switch (language)
                    {
                        case Language.English: return "Edit";
                        case Language.Japanese: return "変更";
                        case Language.Korean: return "변경";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.SaveFilePanelInProject.EditOutputAssetPath":
                    switch (language)
                    {
                        case Language.English: return "Select Output Asset Path";
                        case Language.Japanese: return "エクスポートパス選択";
                        case Language.Korean: return "내보내기 경로 선택";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.Error.OriginFileEmpty":
                    switch (language)
                    {
                        case Language.English: return "Please set \"Origin File\"";
                        case Language.Japanese: return "「元のファイル」を選択してください。";
                        case Language.Korean: return "\"원본 파일\"을 선택해 주세요.";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.Error.DiffFileEmpty":
                    switch (language)
                    {
                        case Language.English: return "\"Diff File\" missing!\nTry re-importing downloaded file and contact the creator if the problem persists.";
                        case Language.Japanese: return "「差分ファイル」が見つかりません。\r\nダウンロードしたファイルを再インポートし、それても問題が解決しない場合は作成者に連絡してください。";
                        case Language.Korean: return "\"차이 파일\"을 찾을 수 없습니다.\r\n다운로드한 파일을 다시 임포트하고, 그럼에도 문제가 지속되면 제작자에게 문의하세요.";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.Error.OutputAssetPathEmpty":
                    switch (language)
                    {
                        case Language.English: return "\"Output Asset Path\" missing!\nTry re-importing downloaded file and contact the creator if the problem persists.";
                        case Language.Japanese: return "「エクスポートパス」が見つかりません。\r\nダウンロードしたファイルを再インポートし、それても問題が解決しない場合は作成者に連絡してください。";
                        case Language.Korean: return "\"내보내기 경로\"을 찾을 수 없습니다.\r\n다운로드한 파일을 다시 임포트하고, 그럼에도 문제가 지속되면 제작자에게 문의하세요.";
                        default: return key;
                    }
                case "AssetPatchAssetEditor.Window.Patch":
                    switch (language)
                    {
                        case Language.English: return "Patch";
                        case Language.Japanese: return "パッチ";
                        case Language.Korean: return "패치";
                        default: return key;
                    }
                #endregion AssetPatchAssetEditor

                #region AssetPatchListAssetEditor
                case "AssetPatchListAssetEditor.Window.Patches":
                    switch (language)
                    {
                        case Language.English: return "Patches";
                        case Language.Japanese: return "パッチファイルのリスト";
                        case Language.Korean: return "패치 파일 목록";
                        default: return key;
                    }
                case "AssetPatchListAssetEditor.Window.PatchAssets.Title":
                    switch (language)
                    {
                        case Language.English: return "Patch #";
                        case Language.Japanese: return "パッチ #";
                        case Language.Korean: return "패치 #";
                        default: return key;
                    }
                case "AssetPatchListAssetEditor.Window.PatchAll":
                    switch (language)
                    {
                        case Language.English: return "Patch All";
                        case Language.Japanese: return "すべてパッチ";
                        case Language.Korean: return "모두 패치";
                        default: return key;
                    }
                #endregion AssetPatchListAssetEditor

                #region AssetPatchCreator
                case "AssetPatchCreator.Progress.CreatePatchFile":
                    switch (language)
                    {
                        case Language.English: return "Create Patch File";
                        case Language.Japanese: return "パッチファイルの作成";
                        case Language.Korean: return "패치 파일 만들기";
                        default: return key;
                    }
                case "AssetPatchCreator.Progress.Working":
                    switch (language)
                    {
                        case Language.English: return "Working...";
                        case Language.Japanese: return "作成中…";
                        case Language.Korean: return "생성 중...";
                        default: return key;
                    }
                #endregion AssetPatchCreator

                #region AssetPatchManager
                case "AssetPatchManager.Dialog.overwriteSameGUIDOutputAsset.Title":
                    switch (language)
                    {
                        case Language.English: return "Asset Patch";
                        case Language.Japanese: return "アセットパッチ";
                        case Language.Korean: return "어셋 패치";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.overwriteSameGUIDOutputAsset.DescFormat":
                    switch (language)
                    {
                        case Language.English: return "Same GUID Asset existing at \"{0}\". overwrite that asset instead output path?";
                        case Language.Japanese: return "同じ GUID のアセットが「{0}」に既に存在しています。 指定されたパスの代わりにこのアセットを上書きしますか？";
                        case Language.Korean: return "동일한 GUID의 어셋이 \"{0}\"에 이미 존재합니다. 지정된 경로 대신에 이를 덮어씌울까요?";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.overwriteSameGUIDOutputAsset.Yes":
                    switch (language)
                    {
                        case Language.English: return "Yes";
                        case Language.Japanese: return "はい";
                        case Language.Korean: return "예";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.overwriteSameGUIDOutputAsset.No":
                    switch (language)
                    {
                        case Language.English: return "No";
                        case Language.Japanese: return "いいえ";
                        case Language.Korean: return "아니요";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.AlreadyExists.Title":
                    switch (language)
                    {
                        case Language.English: return "Asset Patch";
                        case Language.Japanese: return "アセットパッチ";
                        case Language.Korean: return "어셋 패치";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.AlreadyExists.DescFormat":
                    switch (language)
                    {
                        case Language.English: return "Asset \"{0}\" already exists. overwrite?";
                        case Language.Japanese: return "アセット「{0}」が既に存在します。 上書きしますか？";
                        case Language.Korean: return "어셋 \"{0}\"(이)가 이미 존재합니다. 덮어씌울까요?";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.AlreadyExists.Yes":
                    switch (language)
                    {
                        case Language.English: return "Yes";
                        case Language.Japanese: return "はい";
                        case Language.Korean: return "예";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.AlreadyExists.No":
                    switch (language)
                    {
                        case Language.English: return "No";
                        case Language.Japanese: return "いいえ";
                        case Language.Korean: return "아니요";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Error.Title":
                    switch (language)
                    {
                        case Language.English: return "Asset Patch Error";
                        case Language.Japanese: return "アセットパッチエラー";
                        case Language.Korean: return "어셋 패치 실패";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Error.Desc.OriginFileMissing":
                    switch (language)
                    {
                        case Language.English: return "Please set \"Origin File\"";
                        case Language.Japanese: return "「元のファイル」を選択してください。";
                        case Language.Korean: return "\"원본 파일\"을 선택해 주세요.";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Error.Desc.DiffFileMissing":
                    switch (language)
                    {
                        case Language.English: return "\"Diff File\" missing!\nTry re-importing downloaded file and contact the creator if the problem persists.";
                        case Language.Japanese: return "「差分ファイル」が見つかりません。\r\nダウンロードしたファイルを再インポートし、それても問題が解決しない場合は作成者に連絡してください。";
                        case Language.Korean: return "\"차이 파일\"을 찾을 수 없습니다.\r\n다운로드한 파일을 다시 임포트하고, 그럼에도 문제가 지속되면 제작자에게 문의하세요.";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Error.Desc.OutputAssetPathMissing":
                    switch (language)
                    {
                        case Language.English: return "\"Output Asset Path\" missing!\nTry re-importing downloaded file and contact the creator if the problem persists.";
                        case Language.Japanese: return "「エクスポートパス」が見つかりません。\r\nダウンロードしたファイルを再インポートし、それても問題が解決しない場合は作成者に連絡してください。";
                        case Language.Korean: return "\"내보내기 경로\"을 찾을 수 없습니다.\r\n다운로드한 파일을 다시 임포트하고, 그럼에도 문제가 지속되면 제작자에게 문의하세요.";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Error.Ok":
                    switch (language)
                    {
                        case Language.English: return "Ok";
                        case Language.Japanese: return "確認";
                        case Language.Korean: return "확인";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Done.Title":
                    switch (language)
                    {
                        case Language.English: return "Asset Patch";
                        case Language.Japanese: return "アセットパッチ";
                        case Language.Korean: return "어셋 패치";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Done.Desc":
                    switch (language)
                    {
                        case Language.English: return "Done";
                        case Language.Japanese: return "完了";
                        case Language.Korean: return "완료";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.List.Done.DescFormat":
                    switch (language)
                    {
                        case Language.English: return "Done\r\n({successCount} of {totalCount} successful)";
                        case Language.Japanese: return "完了\r\n({totalCount}件中{successCount}件成功)";
                        case Language.Korean: return "완료\r\n({totalCount}개 중 {successCount}개 성공)";
                        default: return key;
                    }
                case "AssetPatchManager.Dialog.Done.Ok":
                    switch (language)
                    {
                        case Language.English: return "Ok";
                        case Language.Japanese: return "確認";
                        case Language.Korean: return "확인";
                        default: return key;
                    }
                #endregion AssetPatchManager

                #region AssetPatchUpdatorWindow
                case "AssetPatchUpdatorWindow.Window.PatchAssets":
                    switch (language)
                    {
                        case Language.English: return "Patches";
                        case Language.Japanese: return "パッチファイルのリスト";
                        case Language.Korean: return "패치 파일 목록";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Window.LoadFromList":
                    switch (language)
                    {
                        case Language.English: return "Load from Patch List";
                        case Language.Japanese: return "パッチリストファイルから読み込む";
                        case Language.Korean: return "패치 리스트 파일에서 불러오기";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Dialog.LoadFromList.Title":
                    switch (language)
                    {
                        case Language.English: return "Done";
                        case Language.Japanese: return "完了";
                        case Language.Korean: return "완료";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Dialog.List.LoadFromList.DescFormat":
                    switch (language)
                    {
                        case Language.English: return "{0} patch files were loaded from the patch list file.";
                        case Language.Japanese: return "パッチリストファイルから{0}個のパッチファイルをインポートしました。";
                        case Language.Korean: return "패치 리스트 파일로부터 {0}개의 패치 파일을 불러왔습니다.";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Dialog.LoadFromList.Ok":
                    switch (language)
                    {
                        case Language.English: return "Ok";
                        case Language.Japanese: return "確認";
                        case Language.Korean: return "확인";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Window.PleaseSelectOneOrMorePatchFiles":
                    switch (language)
                    {
                        case Language.English: return "Please select one or more patch files.";
                        case Language.Japanese: return "1つ以上のパッチファイルを選択してください。";
                        case Language.Korean: return "한 개 이상의 패치 파일을 선택해 주세요.";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Window.UpdatePatches":
                    switch (language)
                    {
                        case Language.English: return "Update patch files";
                        case Language.Japanese: return "パッチファイルの更新";
                        case Language.Korean: return "패치 파일 업데이트";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Dialog.Done.Title":
                    switch (language)
                    {
                        case Language.English: return "Done";
                        case Language.Japanese: return "完了";
                        case Language.Korean: return "완료";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Dialog.List.Done.DescFormat":
                    switch (language)
                    {
                        case Language.English: return "Done\r\n({successCount} of {totalCount} successful)";
                        case Language.Japanese: return "完了\r\n({totalCount}件中{successCount}件成功)";
                        case Language.Korean: return "완료\r\n({totalCount}개 중 {successCount}개 성공)";
                        default: return key;
                    }
                case "AssetPatchUpdatorWindow.Dialog.Done.Ok":
                    switch (language)
                    {
                        case Language.English: return "Ok";
                        case Language.Japanese: return "確認";
                        case Language.Korean: return "확인";
                        default: return key;
                    }
                #endregion AssetPatchUpdatorWindow

                default: return key;
            }
        }
    }
}
